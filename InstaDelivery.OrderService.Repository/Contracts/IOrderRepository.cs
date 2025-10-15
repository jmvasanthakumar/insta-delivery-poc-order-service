using InstaDelivery.OrderService.Domain.Entities;

namespace InstaDelivery.OrderService.Repository.Contracts;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken ct = default);
    Task<IEnumerable<Order>> GetOrderDetails(Guid orderId, CancellationToken ct = default);
}
