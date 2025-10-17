using InstaDelivery.OrderService.Domain.Entities;
using InstaDelivery.OrderService.Repository.Context;
using InstaDelivery.OrderService.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InstaDelivery.OrderService.Repository;

internal class OrderRepository(OrderDbContext dbContext) : GenericRepository<Order>(dbContext), IOrderRepository
{
    public async Task<IEnumerable<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken ct = default)
    {
        return await _set.Where(o => o.CustomerId == customerId).ToListAsync(ct);
    }

    public async Task<IEnumerable<Order>> GetOrderDetails(Guid orderId, CancellationToken ct = default)
    {
        return await _set.Where(o => o.Id == orderId).Include(x => x.Items).ToListAsync(ct);
    }
}