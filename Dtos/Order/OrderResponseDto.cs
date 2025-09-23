using System;
using System.Collections.Generic;

namespace Delivery.Dtos.Order
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    public int EstablishmentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Status { get; set; }
        public List<OrderItemResponseDto>? Items { get; set; }
    }
}
