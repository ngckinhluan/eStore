using BusinessObjects.Entities;
using Repositories.Interface.GenericRepository;

namespace Repositories.Interface;

public interface IOrderRepository : ICreateRepository<Order>, IDeleteRepository<Order>, IUpdateRepository<Order>, IReadRepository<Order>
{
    
}