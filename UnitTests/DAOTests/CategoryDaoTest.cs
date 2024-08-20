using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessObjects.Context;
using BusinessObjects.Entities;
using DAOs;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace UnitTests.DAOTests
{
    public class CategoryDaoTest
    {
        private readonly Mock<DbSet<Category>> _mockSet;
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly List<Category> _categoryList;

        public CategoryDaoTest()
        {
            _mockSet = new Mock<DbSet<Category>>();
            _mockContext = new Mock<ApplicationDbContext>();
            _categoryList = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Beverages" },
                new Category { CategoryId = 2, CategoryName = "Condiments" },
                new Category { CategoryId = 3, CategoryName = "Confections" }
            };
            var queryable = _categoryList.AsQueryable();
            _mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(queryable.Provider);
            _mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(queryable.Expression);
            _mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            _mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            _mockSet.As<IAsyncEnumerable<Category>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Category>(queryable.GetEnumerator()));

            _mockContext.Setup(c => c.Categories).Returns(_mockSet.Object);
            
            // Delete method
            _mockSet.Setup(m => m.Remove(It.IsAny<Category>())).Callback<Category>(author =>
            {
                _categoryList.Remove(author);
            });
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => 1);
            
            // FindAsync method
            _mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) =>
                    _categoryList.SingleOrDefault(a => a.CategoryId == (int)ids[0]));
            
            _mockContext.Setup(c => c.Categories).Returns(_mockSet.Object);
            
            // Update method
            _mockSet.Setup(m => m.Update(It.IsAny<Category>())).Callback<Category>(updatedCategory =>
            {
                var existingCategory = _categoryList.SingleOrDefault(a => a.CategoryId == updatedCategory.CategoryId);
                if (existingCategory != null)
                {
                    existingCategory.CategoryName = updatedCategory.CategoryName;
                }
            });
        }

        [Fact]
        public async Task GetCategories_ReturnsCategories()
        {
            var categoryDao = new CategoryDao(_mockContext.Object);
            var result = await categoryDao.GetCategories();
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetCategoryById_ReturnsCategory()
        {
            var categoryDao = new CategoryDao(_mockContext.Object);
            var result = await categoryDao.GetCategoryById(1);
            Assert.NotNull(result);
            Assert.Equal("Beverages", result?.CategoryName);
        }

        [Fact]
        public async Task AddCategory_AddsCategory()
        {
            var newCategory = new Category { CategoryId = 4, CategoryName = "Dairy" };
            var categoryDao = new CategoryDao(_mockContext.Object);
            await categoryDao.AddCategory(newCategory);
            _mockSet.Verify(m => m.AddAsync(newCategory, It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCategory_UpdatesCategory()
        {
            var updatedCategory = new Category {CategoryId = 1, CategoryName = "Updated Beverages" };
            var categoryDao = new CategoryDao(_mockContext.Object);
            var result = await categoryDao.UpdateCategory(1, updatedCategory);
            Assert.NotNull(result);
            Assert.Equal("Updated Beverages", result?.CategoryName);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCategory_DeletesCategory()
        {
            var categoryDao = new CategoryDao(_mockContext.Object);
            var result = await categoryDao.DeleteCategory(1);
            Assert.Equal(1, result);
            _mockSet.Verify(m => m.Remove(It.IsAny<Category>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}