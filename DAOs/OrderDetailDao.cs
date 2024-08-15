using BusinessObjects.Context;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAOs;

public class OrderDetailDao(ApplicationDbContext context)
{
    private ApplicationDbContext Context { get; } = context;

    public async Task<IEnumerable<OrderDetail>> GetOrderDetails()
    {
        return await Context.OrderDetails.ToListAsync();
    }

    public async Task<OrderDetail?> GetOrderDetailById(int id)
    {
        return await Context.OrderDetails.FindAsync(id);
    }

    public async Task<OrderDetail> AddOrderDetail(OrderDetail orderDetail)
    {
        await Context.OrderDetails.AddAsync(orderDetail);
        await Context.SaveChangesAsync();
        return orderDetail;
    }

    public async Task<OrderDetail?> UpdateOrderDetail(int id, OrderDetail orderDetail)
    {
        var existingOrderDetail = await Context.OrderDetails.FindAsync(id);
        if (existingOrderDetail == null) return null;
        Context.Entry(existingOrderDetail).CurrentValues.SetValues(orderDetail);
        await Context.SaveChangesAsync();
        return existingOrderDetail;
    }

    public async Task<int> DeleteOrderDetail(int id)
    {
        var orderDetail = await Context.OrderDetails.FindAsync(id);
        if (orderDetail == null) return 0;
        Context.OrderDetails.Remove(orderDetail);
        return await Context.SaveChangesAsync();
    }
}