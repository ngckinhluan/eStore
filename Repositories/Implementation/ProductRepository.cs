using BusinessObjects.Entities;
using DAOs;
using Repositories.Interface;

namespace Repositories.Implementation;

public class ProductRepository(ProductDao productDao) : IProductRepository
{
    private ProductDao ProductDao { get; } = productDao;

    public async Task<Product?> AddAsync(Product entity)
    {
        return await ProductDao.AddProduct(entity);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await ProductDao.DeleteProduct(id);
    }

    public async Task<Product?> UpdateAsync(int id, Product entity)
    {
        return await ProductDao.UpdateProduct(id, entity);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await ProductDao.GetProductById(id);
    }

    public async Task<IEnumerable<Product>?> GetAllAsync()
    {
        return await ProductDao.GetProducts();
    }
}
