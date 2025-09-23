namespace Delivery.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    public int? EstablishmentId { get; set; }
        public int? DeliveryPersonId { get; set; }
        public int? OrderId { get; set; }
        public int Rating { get; set; } // 1 a 5
        public string Comment { get; set; } // Changed to non-nullable
        public DateTime CreatedAt { get; set; }
        public User User { get; set; } // Changed to non-nullable
    public Establishment Establishment { get; set; } // Changed to non-nullable
        public DeliveryPerson DeliveryPerson { get; set; } // Changed to non-nullable
        public Order Order { get; set; } // Changed to non-nullable

        // Propriedades auxiliares para facilitar o preenchimento do DTO
        public string ReviewerName { get; set; } // Changed to non-nullable
        public string TargetType { get; set; } // Changed to non-nullable
        public int? TargetId { get; set; }
    }
}
