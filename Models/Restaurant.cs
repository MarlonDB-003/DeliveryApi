namespace Delivery.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Product>? Products { get; set; }

        // Relacionamento com usuário
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
