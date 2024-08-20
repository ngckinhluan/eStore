using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessObjects.Context;
using BusinessObjects.Entities;
using DAOs;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repositories.Implementation;
using Xunit;

namespace UnitTests.RepositoryTests
{
    public class CategoryRepositoryTest
    {
        private readonly Mock<DbSet<Category>> _mockSet;
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly List<Category> _categoryList;
        private readonly CategoryRepository _categoryRepository;

        public CategoryRepositoryTest()
        {
            _mockSet = new Mock<DbSet<Category>>();
            _mockContext = new Mock<ApplicationDbContext>();
            _categoryList = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Category 1" },
                new Category { CategoryId = 2, CategoryName = "Category 2" },
                new Category { CategoryId = 3, CategoryName = "Category 3" }
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
            var categoryDao = new CategoryDao(_mockContext.Object);
            _categoryRepository = new CategoryRepository(categoryDao);

            // Delete method
            _mockSet.Setup(m => m.Remove(It.IsAny<Category>())).Callback<Category>(m => _categoryList.Remove(m));
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // FindAsync method
            _mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => _categoryList.SingleOrDefault(a => a.CategoryId == (int)ids[0]));

            // Update method
            _mockSet.Setup(m => m.Update(It.IsAny<Category>())).Callback<Category>(updatedCategory =>
            {
                var existingCategory = _categoryList.SingleOrDefault(a => a.CategoryId == updatedCategory.CategoryId);
                if (existingCategory != null)
                {
                    existingCategory.CategoryName = updatedCategory.CategoryName;
                }
            });

            // Add method
            _mockSet.Setup(m => m.Add(It.IsAny<Category>())).Callback<Category>(m => _categoryList.Add(m));
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            var result = await _categoryRepository.GetAllAsync();
            Assert.Equal(_categoryList.Count, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCategory()
        {
            var result = await _categoryRepository.GetByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.CategoryId);
        }

        [Fact]
        public async Task AddAsync_AddsCategory()
        {
            // Arrange
            var newCategory = new Category
            {
                CategoryId = 4,
                CategoryName = "Category 4"
            };

            // Act
            await _categoryRepository.AddAsync(newCategory);

            // Assert
            _mockSet.Verify(m => m.AddAsync(newCategory, It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCategory()
        {
            // Arrange
            var updatedCategory = new Category
            {
                CategoryId = 2,
                CategoryName = "Updated Category 2"
            };

            // Act
            await _categoryRepository.UpdateAsync(updatedCategory.CategoryId, updatedCategory);

            // Assert
            var result = await _categoryRepository.GetByIdAsync(2);
            Assert.NotNull(result);
            Assert.Equal(updatedCategory.CategoryName, result.CategoryName);
        }

        [Fact]
        public async Task DeleteAsync_DeletesCategory()
        {
            // Act
            var result = await _categoryRepository.DeleteAsync(1);

            // Assert
            Assert.Equal(1, result); // Verify SaveChangesAsync is called
            Assert.DoesNotContain(_categoryList, c => c.CategoryId == 1);
        }
    }
}