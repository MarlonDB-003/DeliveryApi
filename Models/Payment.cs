namespace Delivery.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string? Method { get; set; } // Ex: Cartão, Dinheiro, Pix
        public string? Status { get; set; } // Ex: Pendente, Pago, Cancelado
        public string? TransactionId { get; set; }
        public Order? Order { get; set; }
    }
}
