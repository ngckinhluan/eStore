using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;

namespace Services.Interface;

public interface ICategoryService
{
    Task<Category?> AddAsync(CategoryRequestDto? entity);
    Task<Category?> UpdateAsync(int id, CategoryRequestDto entity);
    Task<int> DeleteAsync(int id);
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<Category>?> GetAllAsync();
}