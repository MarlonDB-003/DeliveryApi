namespace Delivery.Dtos.User
{
    using System.ComponentModel.DataAnnotations;
    public class UserDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome completo deve ter até 100 caracteres.")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "Telefone inválido.")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        public string? Type { get; set; } // "cliente" ou "estabelecimento"
    }
}
