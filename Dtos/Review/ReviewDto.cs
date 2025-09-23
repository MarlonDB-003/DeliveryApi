namespace Delivery.Dtos.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    public int? EstablishmentId { get; set; }
        public int? DeliveryPersonId { get; set; }
        public int? OrderId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
