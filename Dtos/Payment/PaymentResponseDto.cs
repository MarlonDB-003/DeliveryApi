using System;

namespace Delivery.Dtos.Payment
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Method { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
