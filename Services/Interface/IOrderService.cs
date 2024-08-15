using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;

namespace Services.Interface;

public interface IOrderService
{
    Task<Order?> AddAsync(OrderRequestDto entity);
    Task<Order?> UpdateAsync(int id, OrderRequestDto entity);
    Task<int> DeleteAsync(int id);
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<Order>?> GetAllAsync();
}