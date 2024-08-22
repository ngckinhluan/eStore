using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.DTOs.Response;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using Services.Interface;

namespace eStore.Controllers;
[Route("api/product")]
[ApiController]
public class ProductController(IProductService service, IMapper mapper) : ControllerBase
{
    private IProductService ProductService { get; } = service;
    private IMapper Mapper { get; } = mapper;
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var result = await ProductService.GetAllAsync();
        if (result == null)
        {
            return NotFound();
        }
        var response = Mapper.Map<IEnumerable<ProductResponseDto>>(result);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var result = await ProductService.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        var response = Mapper.Map<ProductResponseDto>(result);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductRequestDto product)
    {
        var result = await ProductService.AddAsync(product);
        if (result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequestDto product)
    {
        var result = await ProductService.UpdateAsync(id, product);
        if (result == null)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await ProductService.DeleteAsync(id);
        if (result == 0)
        {
            return NotFound("Product not found");
        }
        return Ok("Product has been deleted");
    }
}