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
    }
}
