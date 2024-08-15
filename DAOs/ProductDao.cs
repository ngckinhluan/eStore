using BusinessObjects.Context;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAOs;

public class ProductDao(ApplicationDbContext context)
{
   private ApplicationDbContext Context { get; } = context;
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await Context.Products.ToListAsync();
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await Context.Products.FindAsync(id);
    }
    
    public async Task<Product> AddProduct(Product product)
    {
        await Context.Products.AddAsync(product);
        await Context.SaveChangesAsync();
        return product;
    }
    
    public async Task<Product?> UpdateProduct(int id, Product product)
    {
        var existingProduct = await Context.Products.FindAsync(id);
        if (existingProduct == null) return null;
        existingProduct.ProductName = product.ProductName;
        existingProduct.UnitPrice = product.UnitPrice;
        existingProduct.UnitsInStock = product.UnitsInStock;
        existingProduct.Weight = product.Weight;
        await Context.SaveChangesAsync();
        return existingProduct;
    }
    
    public async Task<int> DeleteProduct(int id)
    {
        var product = await Context.Products.FindAsync(id);
        if (product == null) return 0;
        Context.Products.Remove(product);
        return await Context.SaveChangesAsync();
    }
}