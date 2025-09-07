using System;

namespace Delivery.Dtos.Payment
{
    public class PaymentDetailResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Method { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? TransactionId { get; set; }
    }
}
