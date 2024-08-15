using BusinessObjects.Entities;
using DAOs;
using Repositories.Interface;

namespace Repositories.Implementation;

public class OrderRepository(OrderDao orderDao) : IOrderRepository
{
    private OrderDao OrderDao { get; } = orderDao;

    public async Task<Order?> AddAsync(Order entity)
    {
        return await OrderDao.AddOrder(entity);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await OrderDao.DeleteOrder(id);
    }

    public async Task<Order?> UpdateAsync(int id, Order entity)
    {
        return await OrderDao.UpdateOrder(id, entity);
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await OrderDao.GetOrderById(id);
    }

    public async Task<IEnumerable<Order>?> GetAllAsync()
    {
        return await OrderDao.GetOrders();
    }
}