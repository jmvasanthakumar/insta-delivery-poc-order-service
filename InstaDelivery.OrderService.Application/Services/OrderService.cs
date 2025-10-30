using AutoMapper;
using InstaDelivery.OrderService.Application.Dto;
using InstaDelivery.OrderService.Application.Services.Contracts;
using InstaDelivery.OrderService.Domain.Entities;
using InstaDelivery.OrderService.Domain.Exceptions;
using InstaDelivery.OrderService.Repository.Contracts;
using InstaDelivery.Common.Context;
using Microsoft.EntityFrameworkCore;

namespace InstaDelivery.OrderService.Application.Services;

internal class OrderService(IUnitOfWork unitOfWork, IMapper mapper) : IOrderService
{
    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto, CancellationToken ct = default)
    {
        var order = mapper.Map<Order>(orderDto);
        order.CreatedAt = DateTimeOffset.Now;
        order.UpdatedAt = DateTimeOffset.Now;
        order.CreatedBy = RequestContext.Current.UserEmail;
        order.UpdatedBy = RequestContext.Current.UserEmail;
        
        order.Status = OrderStatus.Pending;
        order.TotalAmount = order.Items.Sum(item => item.Subtotal);

        order = await unitOfWork.Orders.AddAsync(order, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return mapper.Map<OrderDto>(order);
    }
    public async Task<IList<OrderDto>> GetOrdersAsync(CancellationToken ct = default)
    {
        var orders = await unitOfWork.Orders.GetAllAsync(ct);
        return mapper.Map<IList<OrderDto>>(orders);
    }

    public async Task<IList<OrderDto>> GetOrdersByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
    {
        var orders = await unitOfWork.Orders.GetOrdersByCustomerId(customerId, ct);
        return mapper.Map<IList<OrderDto>>(orders);
    }

    public async Task<OrderDto> GetOrderByIdAsync(Guid id, CancellationToken ct = default)
    {
        var order = await unitOfWork.Orders.Query().Include(x=>x.Items).SingleOrDefaultAsync(x=>x.Id == id, ct) ?? throw new OrderNotFoundException(id);
        return mapper.Map<OrderDto>(order);
    }

    public async Task<IList<OrderDto>> GetAvailableOrdersAsync(CancellationToken ct = default)
    {
        var availableOrders = await unitOfWork.Orders.FindAsync(x => x.Status == OrderStatus.Pending, ct);
        return mapper.Map<IList<OrderDto>>(availableOrders);
    }

    public async Task<OrderDto> UpdateOrderStatusAsync(Guid orderId, OrderStatus orderStatus, DateTimeOffset updatedAt, CancellationToken ct = default)
    {
        var order = await unitOfWork.Orders.GetByIdAsync(orderId, ct) ?? throw new OrderNotFoundException(orderId);

        order.Status = orderStatus;
        order.UpdatedAt = updatedAt;

        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<OrderDto>(order);
    }

    public async Task DeleteOrderAsync(Guid id, CancellationToken ct = default)
    {
        var order = await unitOfWork.Orders.GetByIdAsync(id, ct) ?? throw new OrderNotFoundException(id);

        unitOfWork.Orders.Remove(order);
        await unitOfWork.SaveChangesAsync(ct);
    }
}
