namespace InstaDelivery.OrderService.Application.Dto;

public class CreateOrderDto : OrderDtoBase
{

    public required ICollection<CreateOrderItemDto> Items { get; set; } = [];

}
