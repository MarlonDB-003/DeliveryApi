namespace Delivery.Models
{
    public class DeliveryPerson
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Vehicle { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public double? AverageRating { get; set; }
        public string? Status { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }

    }
}
