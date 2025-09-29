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
        public DateTime? UpdatedAt { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string Status { get; set; } = "Pendente"; // Pendente, Aceito, EmPreparo, ProntoParaEntrega, ACaminho, Entregue, Cancelado
        public decimal SubTotal { get; set; } // Soma dos valores dos produtos
        public decimal DeliveryFee { get; set; } // Taxa de entrega
        public decimal TotalAmount { get; set; } // SubTotal + DeliveryFee
        public string? DeliveryAddress { get; set; }
        public string? ObservationsForEstablishment { get; set; }
        public string? ObservationsForDelivery { get; set; }
        public string? StatusObservations { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
