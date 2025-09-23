using System;

namespace Delivery.Dtos.Review
{
    public class ReviewDetailResponseDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ReviewerName { get; set; }
    public string? TargetType { get; set; } // Estabelecimento, Entregador, Produto
        public int? TargetId { get; set; }
    }
}
