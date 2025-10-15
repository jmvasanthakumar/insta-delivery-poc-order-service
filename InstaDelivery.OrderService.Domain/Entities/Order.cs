using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstaDelivery.OrderService.Domain.Entities;

public class Order : IEntity
{
    [Key]
    [Column("OrderId")]
    public Guid Id { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public Guid CustomerId { get; set; }

    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public decimal TotalAmount { get; set; }

    public string Currency { get; set; } = "INR";

    public string? DeliveryAddress{ get; set; }

    public Guid? PaymentId { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; } = false;

    public ICollection<OrderItem> Items { get; set; } = [];
    //public ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();
    //public Shipment? Shipment { get; set; }
}

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Shipped = 2,
    Delivered = 3,
    Cancelled = 4
}