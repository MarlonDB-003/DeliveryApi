using Delivery.Models;
using Delivery.Data;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DeliveryContext _context;
        public ProductRepository(DeliveryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product?> FindByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<Product?> FindByNameAndEstablishmentAsync(string name, int establishmentId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Name == name && p.EstablishmentId == establishmentId);
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null) return null;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.ImageUrl = product.ImageUrl;
            existing.CategoryId = product.CategoryId;
            existing.ProductType = product.ProductType;

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
