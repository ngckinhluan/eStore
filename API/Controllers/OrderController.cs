using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.DTOs.Response;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace eStore.Controllers;

[Route("api/order")]
[ApiController]
public class OrderController(IOrderService orderService, IMapper mapper) : ControllerBase
{
    private IOrderService OrderService { get; } = orderService;
    private IMapper Mapper { get; } = mapper;

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var result = await OrderService.GetAllAsync();
        if (result == null)
        {
            return NotFound(new { error = "Order not found!" });
        }

        var response = Mapper.Map<IEnumerable<OrderResponseDto>>(result);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var result = await OrderService.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound(new { error = "Order not found!" });
        }

        var response = Mapper.Map<OrderResponseDto>(result);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] OrderRequestDto order)
    {
        var result = await OrderService.AddAsync(order);
        if (result == null)
        {
            return BadRequest(new { error = "Failed to add order!" });
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderRequestDto order)
    {
        var result = await OrderService.UpdateAsync(id, order);
        if (result == null)
        {
            return BadRequest(new { error = "Failed to update order!" });
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var result = await OrderService.DeleteAsync(id);
        if (result == 0)
        {
            return NotFound(new { error = "Order not found!" });
        }
        return Ok(new { message = "Order has been deleted!" });
    }
}