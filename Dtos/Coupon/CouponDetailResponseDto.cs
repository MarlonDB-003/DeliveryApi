using System;

namespace Delivery.Dtos.Coupon
{
    public class CouponDetailResponseDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public decimal Discount { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
        public int? UsageLimit { get; set; }
    }
}
