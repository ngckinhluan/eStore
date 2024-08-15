using Moq;
using Xunit;
using DAOs;
using BusinessObjects.Context;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class CategoryDaoTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly CategoryDao _categoryDao;
        private readonly DbSet<Category> _mockDbSet;

        public CategoryDaoTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "eStore")
                .Options;

            _mockContext = new Mock<ApplicationDbContext>(options);
            _mockDbSet = new Mock<DbSet<Category>>().Object;

            _mockContext.Setup(c => c.Categories).Returns(_mockDbSet);
            _categoryDao = new CategoryDao(_mockContext.Object);
        }

        [Fact]
        public async Task GetCategories_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Beverages" },
                new Category { CategoryId = 2, CategoryName = "Condiments" }
            };

            _mockContext.Setup(c => c.Categories.ToList()).Returns(categories);
            // Act
            var result = await _categoryDao.GetCategories();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetCategoryById_ReturnsCategory_WhenCategoryExists()
        {
            // Arrange
            var category = new Category { CategoryId = 1, CategoryName = "Beverages" };

            _mockContext.Setup(c => c.Categories.Find(1)).Returns(category);

            // Act
            var result = await _categoryDao.GetCategoryById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Beverages", result.CategoryName);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            _mockContext.Setup(c => c.Categories.Find(1)).Returns((Category)null);

            // Act
            var result = await _categoryDao.GetCategoryById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddCategory_AddsCategorySuccessfully()
        {
            // Arrange
            var category = new Category { CategoryId = 1, CategoryName = "Snacks" };

            _mockContext.Setup(c => c.Categories.AddAsync(category, default));
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _categoryDao.AddCategory(category);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Snacks", result.CategoryName);
        }

        [Fact]
        public async Task UpdateCategory_UpdatesExistingCategory()
        {
            // Arrange
            var existingCategory = new Category { CategoryId = 1, CategoryName = "Beverages" };
            _mockContext.Setup(c => c.Categories.FindAsync(1)).ReturnsAsync(existingCategory);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var updatedCategory = new Category { CategoryId = 1 ,CategoryName = "Drinks" };

            // Act
            var result = await _categoryDao.UpdateCategory(1, updatedCategory);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Drinks", result.CategoryName);
        }

        [Fact]
        public async Task DeleteCategory_RemovesCategorySuccessfully()
        {
            // Arrange
            var category = new Category { CategoryId = 1, CategoryName = "Beverages" };

            _mockContext.Setup(c => c.Categories.FindAsync(1)).ReturnsAsync(category);
            _mockContext.Setup(c => c.Categories.Remove(category));
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _categoryDao.DeleteCategory(1);

            // Assert
            Assert.Equal(1, result);
        }
    }
}
