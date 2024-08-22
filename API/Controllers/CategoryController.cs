using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.DTOs.Response;
using BusinessObjects.Entities;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Interface;

namespace eStore.Controllers;

[Route("api/category")]
[ApiController]
public class CategoryController(ICategoryService categoryService, IMapper mapper, ILoggerManager logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var result = await categoryService.GetAllAsync();
            logger.LogInfo($"Return all categories from database");
            var response = mapper.Map<IEnumerable<CategoryResponseDto>>(result);
            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong inside GetAllCategories action: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}", Name = "GetCategoryById")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var result = await categoryService.GetByIdAsync(id);
        if (result == null)
        {
            logger.LogError($"Category with id: {id} was not found in the database.");
            return NotFound(new { message = "Category not found" });
        }

        var response = mapper.Map<CategoryResponseDto>(result);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDto? category)
    {
        try
        {
            if (category == null)
            {
                logger.LogError("Category object sent from client is null.");
                return BadRequest("Category object is null");
            }

            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid category object sent from client.");
                return BadRequest("Invalid model object");
            }
            // var categoryEntity = mapper.Map<Category>(category);
            var createdCategory = await categoryService.AddAsync(category);
            return CreatedAtRoute("GetCategoryById", new { id = createdCategory?.CategoryId }, createdCategory);
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong inside CreateCategory action: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequestDto category)
    {
        if (category == null || id <= 0)
        {
            return BadRequest(new { message = "Invalid category data" });
        }

        var result = await categoryService.UpdateAsync(id, category);
        if (result == null)
        {
            return BadRequest(new { message = "Failed to update category" });
        }

        return Ok(new { message = "Category has been updated", result });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await categoryService.DeleteAsync(id);
        if (result == 0)
        {
            return NotFound(new { message = "Category not found" });
        }

        return Ok(new { message = "Category has been deleted" });
    }
}