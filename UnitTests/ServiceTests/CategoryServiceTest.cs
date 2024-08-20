using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;
using Moq;
using Repositories.Interface;
using Services.Implementation;
using Xunit;

namespace UnitTests.ServiceTests
{
    public class CategoryServiceTest
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoryService _categoryService;
        private readonly List<Category> _categoryList;

        public CategoryServiceTest()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _categoryService = new CategoryService(_mockRepository.Object, _mockMapper.Object);

            _categoryList = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Category 1" },
                new Category { CategoryId = 2, CategoryName = "Category 2" }
            };
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_categoryList);

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            Assert.Equal(_categoryList.Count, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCategory()
        {
            // Arrange
            var category = _categoryList[0];
            _mockRepository.Setup(repo => repo.GetByIdAsync(category.CategoryId)).ReturnsAsync(category);

            // Act
            var result = await _categoryService.GetByIdAsync(category.CategoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.CategoryId, result.CategoryId);
        }

        [Fact]
        public async Task AddAsync_AddsCategory()
        {
            // Arrange
            var categoryRequest = new CategoryRequestDto { CategoryName = "Category 3" };
            var category = new Category { CategoryId = 3, CategoryName = categoryRequest.CategoryName };
            
            _mockMapper.Setup(m => m.Map<Category>(categoryRequest)).Returns(category);
            _mockRepository.Setup(repo => repo.AddAsync(category)).ReturnsAsync(category);

            // Act
            var result = await _categoryService.AddAsync(categoryRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.CategoryName, result.CategoryName);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCategory()
        {
            // Arrange
            var categoryRequest = new CategoryRequestDto { CategoryName = "Updated Category 1" };
            var updatedCategory = new Category { CategoryId = 1, CategoryName = categoryRequest.CategoryName };

            _mockMapper.Setup(m => m.Map<Category>(categoryRequest)).Returns(updatedCategory);
            _mockRepository.Setup(repo => repo.UpdateAsync(updatedCategory.CategoryId, updatedCategory)).ReturnsAsync(updatedCategory);

            // Act
            var result = await _categoryService.UpdateAsync(updatedCategory.CategoryId, categoryRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedCategory.CategoryName, result.CategoryName);
        }

        [Fact]
        public async Task DeleteAsync_DeletesCategory()
        {
            // Arrange
            var categoryId = 1;
            _mockRepository.Setup(repo => repo.DeleteAsync(categoryId)).ReturnsAsync(1);

            // Act
            var result = await _categoryService.DeleteAsync(categoryId);

            // Assert
            Assert.Equal(1, result);
        }
    }
}