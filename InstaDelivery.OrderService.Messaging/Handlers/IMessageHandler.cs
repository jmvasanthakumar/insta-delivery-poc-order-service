namespace InstaDelivery.OrderService.Messaging.Handlers;

internal interface IMessageHandler<T>
{
    Task HandleAsync(T message, CancellationToken ct);
}
