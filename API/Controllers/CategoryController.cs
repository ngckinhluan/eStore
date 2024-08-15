using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.DTOs.Response;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace eStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(ICategoryService categoryService, IMapper mapper) : ControllerBase
{
    private ICategoryService CategoryService { get; } = categoryService;
    private IMapper Mapper { get; } = mapper;

    [HttpGet("GetCategories")]
    public async Task<IActionResult> GetCategories()
    {
        var result = await CategoryService.GetAllAsync();
        if (result == null)
        {
            return NotFound();
        }
        var response = Mapper.Map<IEnumerable<CategoryResponseDto>>(result);
        return Ok(response);
    }

    [HttpGet("GetCategoryById/{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var result = await CategoryService.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound(new { message = "Category not found" });
        }
        var response = Mapper.Map<CategoryResponseDto>(result);
        return Ok(result);
    }

    [HttpPost("AddCategory")]
    public async Task<IActionResult> AddCategory([FromBody] CategoryRequestDto category)
    {
        var result = await CategoryService.AddAsync(category);
        if (result == null)
        {
            return BadRequest(new { message = "Failed to add category" });
        }

        return Ok(result);
    }

    [HttpPut("UpdateCategory/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequestDto category)
    {
        var result = await CategoryService.UpdateAsync(id, category);
        if (result == null)
        {
            return BadRequest(new { message = "Failed to update category" });
        }

        return Ok(new { message = "Category has been updated", result });
    }

    [HttpDelete("DeleteCategory/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await CategoryService.DeleteAsync(id);
        if (result == 0)
        {
            return NotFound(new { message = "Category not found" });
        }
        return Ok(new { message = "Category has been deleted" });
    }

}