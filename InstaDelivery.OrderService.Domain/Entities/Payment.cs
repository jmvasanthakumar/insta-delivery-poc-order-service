using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstaDelivery.OrderService.Domain.Entities
{
    public class Payment : IEntity
    {
        [Key]
        [Column("PaymentId")]
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string? TransactionReference { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "INR";

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public DateTimeOffset? PaidAt { get; set; }

        public string? FailureReason { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public Order? Order { get; set; }
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Paid = 1,
        Failed = 2,
        Refunded = 3
    }
}