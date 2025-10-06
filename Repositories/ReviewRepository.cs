using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Data;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DeliveryContext _context;
        public ReviewRepository(DeliveryContext context)
        {
            _context = context;
        }

        public async Task<Review?> UpdateAsync(int id, Review review)
        {
            var entity = await _context.Reviews.FindAsync(id);
            if (entity == null) return null;
            entity.UserId = review.UserId;
            entity.OrderId = review.OrderId;
            entity.Rating = review.Rating;
            entity.Comment = review.Comment;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task<Review> AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Review?> FindByUserAndOrderAsync(int userId, int orderId)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.UserId == userId && r.OrderId == orderId);
        }
    }
}
