using Delivery.Models;
using Delivery.Repositories.Interfaces;
using Delivery.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly DeliveryContext _context;
        public OrderItemRepository(DeliveryContext context)
        {
            _context = context;
        }

        public async Task<OrderItem?> UpdateAsync(int id, OrderItem item)
        {
            var entity = await _context.OrderItems.FindAsync(id);
            if (entity == null) return null;
            entity.OrderId = item.OrderId;
            entity.ProductId = item.ProductId;
            entity.Quantity = item.Quantity;
            entity.UnitPrice = item.UnitPrice;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _context.OrderItems.ToListAsync();
        }

        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

        public async Task<OrderItem> AddAsync(OrderItem item)
        {
            _context.OrderItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item == null) return false;
            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
