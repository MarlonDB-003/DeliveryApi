namespace Delivery.Dtos.Establishment
{
    public class EstablishmentRegisterDto
    {
        // Dados do usuário
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "estabelecimento";

    // Dados do restaurante
    public string RestaurantName { get; set; } = string.Empty;
    public EstablishmentAddressDto? Address { get; set; }
    public int? CategoryId { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public bool HasDeliveryPerson { get; set; }
    public decimal MinimumOrderValue { get; set; }
    public decimal DeliveryFee { get; set; }
    }
}
