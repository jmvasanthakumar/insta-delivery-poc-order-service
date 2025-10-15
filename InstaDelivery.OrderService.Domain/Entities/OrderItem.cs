using System.ComponentModel.DataAnnotations.Schema;

namespace InstaDelivery.OrderService.Domain.Entities;

public class OrderItem:IEntity
{
    [Column("OrderItemId")]
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [NotMapped]
    public decimal Subtotal => UnitPrice * Quantity;

    public Order? Order { get; set; }
}