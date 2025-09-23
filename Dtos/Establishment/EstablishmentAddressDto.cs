namespace Delivery.Dtos.Establishment
{
    public class EstablishmentAddressDto
    {
        public string? Description { get; set; } // Ex: Comercial, Matriz, etc
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Complement { get; set; }
    }
}
