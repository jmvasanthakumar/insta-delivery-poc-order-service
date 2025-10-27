namespace InstaDelivery.OrderService.Messaging.Handlers.Contracts;

internal interface IMessageHandler<T>
{
    Task HandleAsync(T message, CancellationToken ct);
}
