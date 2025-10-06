namespace Delivery.Dtos.Establishment
{
    using System.ComponentModel.DataAnnotations;
    public class EstablishmentDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome do estabelecimento é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter até 100 caracteres.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(255, ErrorMessage = "A descrição deve ter até 255 caracteres.")]
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "O horário de abertura é obrigatório.")]
        public TimeSpan OpeningTime { get; set; }
        [Required(ErrorMessage = "O horário de fechamento é obrigatório.")]
        public TimeSpan ClosingTime { get; set; }
        public bool HasDeliveryPerson { get; set; }
        [Required(ErrorMessage = "O valor mínimo do pedido é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O valor mínimo deve ser positivo.")]
        public decimal MinimumOrderValue { get; set; }
        [Required(ErrorMessage = "A taxa de entrega é obrigatória.")]
        [Range(0, double.MaxValue, ErrorMessage = "A taxa de entrega deve ser positiva.")]
        public decimal DeliveryFee { get; set; }
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        public string? PasswordHash { get; set; }
    }
}
