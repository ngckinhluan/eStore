using BusinessObjects.Entities;
using DAOs;
using Repositories.Interface;

namespace Repositories.Implementation;

public class CategoryRepository(CategoryDao categoryDao) : ICategoryRepository
{
    private CategoryDao CategoryDao { get; } = categoryDao;
    public async Task<Category?> AddAsync(Category entity)
    {
        return await CategoryDao.AddCategory(entity);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await CategoryDao.DeleteCategory(id);
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await CategoryDao.GetCategoryById(id);
    }

    public async Task<IEnumerable<Category>?> GetAllAsync()
    {
        return await CategoryDao.GetCategories();
    }

    public async Task<Category?> UpdateAsync(int id, Category entity)
    {
        return await CategoryDao.UpdateCategory(id, entity);
    }
}