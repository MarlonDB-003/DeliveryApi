namespace Delivery.Dtos.Product
{
    using System.ComponentModel.DataAnnotations;
    public class ProductUpdateDto
    {
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter até 100 caracteres.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(255, ErrorMessage = "A descrição deve ter até 255 caracteres.")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "O tipo de produto é obrigatório.")]
        public string ProductType { get; set; } // "Normal", "Combo" ou "Promocao"
    }
}