using System;
using System.Collections.Generic;
using Delivery.Dtos.Address;

namespace Delivery.Dtos.User
{
    public class UserDetailResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Status { get; set; }
        public List<string>? Phones { get; set; }
        public List<AddressResponseDto>? Addresses { get; set; }
    }
}
