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
    public string? Address { get; set; }
    public int? CategoryId { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public bool HasDeliveryPerson { get; set; }
    public decimal MinimumOrderValue { get; set; }
    public decimal DeliveryFee { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Status { get; set; }
    public double? AverageRating { get; set; }
    public List<ProductResponseDto>? Products { get; set; }
    public List<CategoryResponseDto>? Categories { get; set; }
    }
}
