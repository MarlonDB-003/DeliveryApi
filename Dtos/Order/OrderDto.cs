using System;
using System.Collections.Generic;

namespace Delivery.Dtos.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Status { get; set; }
        public List<OrderItemDto>? Items { get; set; }
    }
}
