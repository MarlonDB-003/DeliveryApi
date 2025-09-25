using Delivery.Models;
using Delivery.Data;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Repositories
{
    public class EstablishmentRepository : IEstablishmentRepository
    {
        private readonly DeliveryContext _context;
        public EstablishmentRepository(DeliveryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Establishment>> GetAllAsync()
        {
            return await _context.Establishments.ToListAsync();
        }

        public async Task<Establishment?> GetByIdAsync(int id)
        {
            return await _context.Establishments.FindAsync(id);
        }

        public async Task<Establishment> AddAsync(Establishment establishment)
        {
            _context.Establishments.Add(establishment);
            await _context.SaveChangesAsync();

            // Salvar Address manualmente se existir e não foi salvo em cascade
            if (establishment.User != null && establishment.User.Id > 0 && establishment.Address != null)
            {
                var address = new Address
                {
                    Description = establishment.Address,
                    EstablishmentId = establishment.Id,
                    IsMain = true
                };
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
            }

            return establishment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var establishment = await _context.Establishments.FindAsync(id);
            if (establishment == null) return false;
            _context.Establishments.Remove(establishment);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }
    }
}
