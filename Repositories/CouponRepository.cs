using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Data;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly DeliveryContext _context;
        public CouponRepository(DeliveryContext context)
        {
            _context = context;
        }

        public async Task<Coupon?> UpdateAsync(int id, Coupon coupon)
        {
            var entity = await _context.Coupons.FindAsync(id);
            if (entity == null) return null;
            entity.Code = coupon.Code;
            entity.Discount = coupon.Discount;
            entity.ValidUntil = coupon.ValidUntil;
            entity.IsActive = coupon.IsActive;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            return await _context.Coupons.ToListAsync();
        }

        public async Task<Coupon?> GetByIdAsync(int id)
        {
            return await _context.Coupons.FindAsync(id);
        }

        public async Task<Coupon> AddAsync(Coupon coupon)
        {
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();
            return coupon;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null) return false;
            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Coupon?> FindByCodeAsync(string code)
        {
            return await _context.Coupons.FirstOrDefaultAsync(c => c.Code == code);
        }
    }
}
