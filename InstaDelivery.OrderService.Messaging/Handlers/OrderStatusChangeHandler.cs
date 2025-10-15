using InstaDelivery.OrderService.Application.Services.Contracts;
using InstaDelivery.OrderService.Domain.Entities;
using InstaDelivery.OrderService.Messaging.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InstaDelivery.OrderService.Messaging.Handlers;

public class OrderStatusChangeHandler
    (IServiceScopeFactory serviceScopeFactory,
    ILogger<OrderStatusChangeHandler> logger)
    : IMessageHandler<OrderStatusChange>
{
    public async Task HandleAsync(OrderStatusChange message, CancellationToken ct)
    {
        logger.LogInformation("Processing DeliveryStatusChanged for order {OrderId}", message.OrderId);
        if (Enum.TryParse<OrderStatus>(message.Status, true, out var status) == false)
        {
            logger.LogWarning("Invalid status {Status} for order {OrderId}", message.Status, message.OrderId);
            return;
        }
        using var scope = serviceScopeFactory.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
        await orderService.UpdateOrderStatusAsync(message.OrderId, status, message.ChangedAt, ct);
        logger.LogInformation("Order {OrderId} updated to {Status}", message.OrderId, status);
    }
}
