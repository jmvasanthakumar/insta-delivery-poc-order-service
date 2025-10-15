using InstaDelivery.OrderService.Domain.Entities;

namespace InstaDelivery.OrderService.Application.Dto
{
    public class OrderDto : OrderDtoBase
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<OrderItemDto> Items { get; set; } = [];
    }
}
