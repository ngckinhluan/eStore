using BusinessObjects.Entities;
using Repositories.Interface.GenericRepository;

namespace Repositories.Interface;

public interface IProductRepository : ICreateRepository<Product>, IDeleteRepository<Product>, IReadRepository<Product>, IUpdateRepository<Product>
{
    
}