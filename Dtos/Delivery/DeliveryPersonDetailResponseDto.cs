namespace Delivery.Dtos.Delivery
{
    public class DeliveryPersonDetailResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Vehicle { get; set; }
        public string? ImageUrl { get; set; }
        public double? AverageRating { get; set; }
        public string? Status { get; set; }
    }
}
