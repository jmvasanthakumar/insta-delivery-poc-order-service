using InstaDelivery.OrderService.Application.Dto;
using InstaDelivery.OrderService.Domain.Entities;

namespace InstaDelivery.OrderService.Application.Services.Contracts
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto, CancellationToken ct = default);
        Task<OrderDto?> GetOrderByIdAsync(Guid id, CancellationToken ct = default);
        Task<IList<OrderDto>> GetOrdersByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
        Task<IList<OrderDto>> GetOrdersAsync(CancellationToken ct = default);
        Task<OrderDto> UpdateOrderStatusAsync(Guid orderId, OrderStatus orderStatus, DateTimeOffset updatedAt, CancellationToken ct = default);
    }
}