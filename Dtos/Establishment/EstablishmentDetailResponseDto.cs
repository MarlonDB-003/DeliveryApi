using System.Collections.Generic;
using Delivery.Dtos.Address;
using Delivery.Dtos.Product;
using Delivery.Dtos.Category;

namespace Delivery.Dtos.Establishment
{
    public class EstablishmentDetailResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public AddressResponseDto? Address { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
        public string? OpeningHours { get; set; }
        public double? AverageRating { get; set; }
        public List<ProductResponseDto>? Products { get; set; }
        public List<CategoryResponseDto>? Categories { get; set; }
    }
}
