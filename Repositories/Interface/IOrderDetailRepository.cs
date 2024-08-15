using BusinessObjects.Entities;
using Repositories.Interface.GenericRepository;

namespace Repositories.Interface;

public interface IOrderDetailRepository : ICreateRepository<OrderDetail>, IDeleteRepository<OrderDetail>, IReadRepository<OrderDetail>, IUpdateRepository<OrderDetail>
{
    
}