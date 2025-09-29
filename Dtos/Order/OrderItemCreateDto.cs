using System.ComponentModel.DataAnnotations;

namespace Delivery.Dtos.Order
{
    public class OrderItemCreateDto
    {
        [Required(ErrorMessage = "ID do produto é obrigatório")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantidade é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
        public int Quantity { get; set; }

        public string? Observations { get; set; }
    }
}