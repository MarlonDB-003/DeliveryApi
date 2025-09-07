using Delivery.Dtos.Category;

namespace Delivery.Dtos.Product
{
    public class ProductDetailResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public CategoryResponseDto? Category { get; set; }
        public bool IsAvailable { get; set; }
        public double? AverageRating { get; set; }
    }
}
