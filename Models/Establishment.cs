namespace Delivery.Models
{
    public class Establishment
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

    // Novos campos
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public bool HasDeliveryPerson { get; set; }
    public decimal MinimumOrderValue { get; set; }
    public decimal DeliveryFee { get; set; }
    }
}
