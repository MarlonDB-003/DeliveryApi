namespace Delivery.Dtos.Review
{
    using System.ComponentModel.DataAnnotations;
    public class ReviewDto
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public int? EstablishmentId { get; set; }
        public int? DeliveryPersonId { get; set; }
        public int? OrderId { get; set; }
        [Required(ErrorMessage = "A nota é obrigatória.")]
        [Range(1, 5, ErrorMessage = "A nota deve ser entre 1 e 5.")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "O comentário é obrigatório.")]
        [StringLength(255, ErrorMessage = "O comentário deve ter até 255 caracteres.")]
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
