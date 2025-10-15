namespace InstaDelivery.OrderService.Application.Dto;

public class CreateOrderItemDto
{

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public required string ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }


}
