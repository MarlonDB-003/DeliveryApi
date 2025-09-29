using System.ComponentModel.DataAnnotations;

namespace Delivery.Dtos.Order
{
    public class OrderCreateDto
    {
        [Required(ErrorMessage = "ID do estabelecimento é obrigatório")]
        public int EstablishmentId { get; set; }

        [Required(ErrorMessage = "Itens do pedido são obrigatórios")]
        [MinLength(1, ErrorMessage = "O pedido deve conter pelo menos um item")]
        public List<OrderItemCreateDto> Items { get; set; } = new List<OrderItemCreateDto>();

        public string? DeliveryAddress { get; set; }
        public decimal? DeliveryFee { get; set; } // Opcional - se não informado, será calculado automaticamente
        public string? ObservationsForEstablishment { get; set; }
        public string? ObservationsForDelivery { get; set; }
    }
}