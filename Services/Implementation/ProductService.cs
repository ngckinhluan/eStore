using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;
using Repositories.Implementation;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implementation;

public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
{
    private IProductRepository ProductRepository { get; } = productRepository;
    private IMapper Mapper { get; } = mapper;

    public async Task<Product?> AddAsync(ProductRequestDto entity)
    {
        var promotion = Mapper.Map<Product>(entity);
        return await ProductRepository.AddAsync(promotion);
    }

    public Task<Product?> UpdateAsync(int id, ProductRequestDto entity)
    {
        var promotion = Mapper.Map<Product>(entity);
        return ProductRepository.UpdateAsync(id, promotion);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await ProductRepository.DeleteAsync(id);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await ProductRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Product>?> GetAllAsync()
    {
        return await ProductRepository.GetAllAsync();
    }
}