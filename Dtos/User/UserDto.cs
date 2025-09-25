namespace Delivery.Dtos.User
{
    public class UserDto
    {
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Password { get; set; }
    public string? Type { get; set; } // "cliente" ou "estabelecimento"
    }
}
