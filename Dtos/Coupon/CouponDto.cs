namespace Delivery.Dtos.Coupon
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public decimal Discount { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsActive { get; set; }
    }
}
