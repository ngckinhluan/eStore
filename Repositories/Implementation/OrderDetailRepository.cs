using BusinessObjects.Entities;
using DAOs;
using Repositories.Interface;

namespace Repositories.Implementation;

public class OrderDetailRepository(OrderDetailDao orderDetailDao) : IOrderDetailRepository
{
    private OrderDetailDao OrderDetailDao { get; } = orderDetailDao;

    public async Task<OrderDetail?> AddAsync(OrderDetail entity)
    {
        return await OrderDetailDao.AddOrderDetail(entity);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await OrderDetailDao.DeleteOrderDetail(id);
    }

    public async Task<OrderDetail?> UpdateAsync(int id, OrderDetail entity)
    {
        return await OrderDetailDao.UpdateOrderDetail(id, entity);
    }

    public async Task<OrderDetail?> GetByIdAsync(int id)
    {
        return await OrderDetailDao.GetOrderDetailById(id);
    }

    public async Task<IEnumerable<OrderDetail>?> GetAllAsync()
    {
        return await OrderDetailDao.GetOrderDetails();
    }
}