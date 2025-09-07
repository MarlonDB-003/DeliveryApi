using System;
using System.Collections.Generic;
using Delivery.Dtos.User;
using Delivery.Dtos.Restaurant;
using Delivery.Dtos.Delivery;
using Delivery.Dtos.Address;

namespace Delivery.Dtos.Order
{
    public class OrderDetailResponseDto
    {
        public int Id { get; set; }
        public UserResponseDto? User { get; set; }
        public RestaurantResponseDto? Restaurant { get; set; }
        public DeliveryPersonResponseDto? DeliveryPerson { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Status { get; set; }
        public decimal TotalValue { get; set; }
        public string? PaymentMethod { get; set; }
        public AddressResponseDto? DeliveryAddress { get; set; }
        public List<OrderItemResponseDto>? Items { get; set; }
        public string? Notes { get; set; }
    }
}
