namespace InstaDelivery.OrderService.Application.Dto;

public class OrderDtoBase
{
    public string OrderNumber { get; set; } = string.Empty;

    public Guid CustomerId { get; set; }

    public DateTimeOffset OrderDate { get; set; }
    public required string Currency { get; set; }

    public required OrderAddressDto DeliveryAddress { get; set; }
    public Guid? PaymentId { get; set; }

}
