using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;

namespace Services.Interface;

public interface IProductService
{
    Task<Product?> AddAsync(ProductRequestDto entity);
    Task<Product?> UpdateAsync(int id, ProductRequestDto entity);
    Task<int> DeleteAsync(int id);
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>?> GetAllAsync();
}