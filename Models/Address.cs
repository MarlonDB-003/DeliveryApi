namespace Delivery.Models
{
    public class Address
    {
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int? EstablishmentId { get; set; }
    public string? Description { get; set; } // Ex: Casa, Trabalho, Outro
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? Neighborhood { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Complement { get; set; }
    public bool IsMain { get; set; }
    public User? User { get; set; }
    public Establishment? Establishment { get; set; }
    }
}
