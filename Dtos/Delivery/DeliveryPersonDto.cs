namespace Delivery.Dtos.Delivery
{
    using System.ComponentModel.DataAnnotations;
    public class DeliveryPersonDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter até 100 caracteres.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "Telefone inválido.")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "O veículo é obrigatório.")]
        public string? Vehicle { get; set; }
        public string? ImageUrl { get; set; }
    }
}
