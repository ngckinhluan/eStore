using BusinessObjects.Context;
using BusinessObjects.DTOs.Response;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAOs;

public class OrderDao(ApplicationDbContext context)
{
    private ApplicationDbContext Context { get; } = context;

    public async Task<IEnumerable<Order>> GetOrders()
    {
        return await Context.Orders.Include(o => o.OrderDetails).ToListAsync();
    }

    public async Task<Order?> GetOrderById(int id)
    {
        return await Context.Orders.FindAsync(id);
    }

    public async Task<Order> AddOrder(Order order)
    {
        await Context.Orders.AddAsync(order);
        await Context.SaveChangesAsync();
        return order;
    }

    public async Task<int> DeleteOrder(int id)
    {
        var order = await Context.Orders.FindAsync(id);
        if (order == null) return 0;
        Context.Orders.Remove(order);
        return await Context.SaveChangesAsync();
    }

    public async Task<Order?> UpdateOrder(int id, Order order)
    {
        var existingOrder = await Context.Orders.FindAsync(id);
        if (existingOrder == null) return null;
        existingOrder.Freight = order.Freight;
        existingOrder.OrderDate = order.OrderDate;
        existingOrder.RequiredDate = order.RequiredDate;
        existingOrder.ShippedDate = order.ShippedDate;
        await Context.SaveChangesAsync();
        return existingOrder;
    }
}