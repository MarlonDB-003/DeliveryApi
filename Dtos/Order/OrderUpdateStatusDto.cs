namespace Delivery.Dtos.Order
{
    public class OrderUpdateStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public int? DeliveryPersonId { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
        public string? StatusObservations { get; set; }
    }
}