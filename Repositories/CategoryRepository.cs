using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Data;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DeliveryContext _context;
        public CategoryRepository(DeliveryContext context)
        {
            _context = context;
        }

        public async Task<Category?> UpdateAsync(int id, Category category)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null) return null;
            entity.Name = category.Name;
            entity.Description = category.Description;
            await _context.SaveChangesAsync();
            return entity;
        }
        // ...existing code...

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Category?> FindByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
