using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implementation;

public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper) : ICategoryService
{
    private ICategoryRepository CategoryRepository { get; } = categoryRepository;
    private IMapper Mapper { get; } = mapper;

    public Task<Category?> AddAsync(CategoryRequestDto entity)
    {
        var category = Mapper.Map<Category>(entity);
        return CategoryRepository.AddAsync(category);
    }

    public async Task<Category?> UpdateAsync(int id, CategoryRequestDto entity)
    {
        return await CategoryRepository.UpdateAsync(id, Mapper.Map<Category>(entity));
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await CategoryRepository.DeleteAsync(id);
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await CategoryRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Category>?> GetAllAsync()
    {
        return await CategoryRepository.GetAllAsync();
    }
}