namespace InstaDelivery.OrderService.Application.Dto;

public class OrderItemDto : CreateOrderItemDto
{
    public Guid Id { get; set; }
    public decimal Subtotal { get; set; }
}