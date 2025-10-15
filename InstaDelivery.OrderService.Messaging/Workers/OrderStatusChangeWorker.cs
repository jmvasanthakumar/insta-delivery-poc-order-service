using Azure.Messaging.ServiceBus;
using InstaDelivery.OrderService.Messaging.Domain;
using InstaDelivery.OrderService.Messaging.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InstaDelivery.OrderService.Messaging.Workers;

internal class OrderStatusChangeWorker(
    IMessageHandler<OrderStatusChange> handler,
    ILogger<OrderStatusChangeWorker> _logger,
    [FromKeyedServices("OrderServiceBus")] ServiceBusClient client) : BackgroundService
{
    private ServiceBusProcessor? _processor;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _processor = client.CreateProcessor("order-events", new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 4
        });

        _processor.ProcessMessageAsync += ProcessMessageAsync;
        _processor.ProcessErrorAsync += ErrorHandlerAsync;

        await _processor.StartProcessingAsync(cancellationToken);
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        var ct = args.CancellationToken;
        try
        {
            var dto = JsonSerializer.Deserialize<OrderStatusChange>(args.Message.Body.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (dto == null)
            {
                await args.DeadLetterMessageAsync(args.Message, "InvalidPayload");
                return;
            }

            await handler.HandleAsync(dto, ct);

            await args.CompleteMessageAsync(args.Message, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Processing failed for message {MessageId}", args.Message.MessageId);
            try { await args.AbandonMessageAsync(args.Message, cancellationToken: ct); } catch { }
        }
    }

    private Task ErrorHandlerAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Service bus error {Entity}", args.EntityPath);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_processor != null)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            await _processor.DisposeAsync();
            _processor = null;
        }
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;
}
