namespace Delivery.Dtos.Address
{
    using System.ComponentModel.DataAnnotations;
    public class AddressDto
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required(ErrorMessage = "A rua é obrigatória.")]
        public string? Street { get; set; }
        [Required(ErrorMessage = "O número é obrigatório.")]
        public string? Number { get; set; }
        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string? Neighborhood { get; set; }
        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string? City { get; set; }
        [Required(ErrorMessage = "O estado é obrigatório.")]
        public string? State { get; set; }
        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [StringLength(10, ErrorMessage = "O CEP deve ter até 10 caracteres.")]
        public string? ZipCode { get; set; }
        public string? Complement { get; set; }
        public bool IsMain { get; set; }
    }
}
