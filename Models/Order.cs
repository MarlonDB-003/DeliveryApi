namespace Delivery.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int EstablishmentId { get; set; }
        public Establishment? Establishment { get; set; }
        public int? DeliveryPersonId { get; set; }
        public DeliveryPerson? DeliveryPerson { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Status { get; set; } // Ex: Pendente, Em Preparo, A Caminho, Entregue
        public List<OrderItem>? OrderItems { get; set; }
    }
}
