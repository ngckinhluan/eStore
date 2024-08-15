using System.Linq.Expressions;
using BusinessObjects.Context;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAOs;

public class CategoryDao(ApplicationDbContext context)
{
    private ApplicationDbContext Context { get; } = context;
    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await Context.Categories.ToListAsync();
    }

    public async Task<Category?> GetCategoryById(int id)
    {
        return await Context.Categories.FindAsync(id);
    }

    public async Task<Category> AddCategory(Category category)
    {
        await Context.Categories.AddAsync(category);
        await Context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> UpdateCategory(int id, Category category)
    {
        var existingCategory = await Context.Categories.FindAsync(id);
        if (existingCategory == null) return null;
        existingCategory.CategoryName = category.CategoryName;
        await Context.SaveChangesAsync();
        return existingCategory;
    }


    public async Task<int> DeleteCategory(int id)
    {
        var category = await Context.Categories.FindAsync(id);
        if (category == null) return 0;
        Context.Categories.Remove(category);
        return await Context.SaveChangesAsync();
    }
}