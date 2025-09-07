using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Data;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Repositories
{
    public class DeliveryPersonRepository : IDeliveryPersonRepository
    {
        private readonly DeliveryContext _context;
        public DeliveryPersonRepository(DeliveryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeliveryPerson>> GetAllAsync()
        {
            return await _context.DeliveryPeople.ToListAsync();
        }

        public async Task<DeliveryPerson?> GetByIdAsync(int id)
        {
            return await _context.DeliveryPeople.FindAsync(id);
        }

        public async Task<DeliveryPerson> AddAsync(DeliveryPerson deliveryPerson)
        {
            _context.DeliveryPeople.Add(deliveryPerson);
            await _context.SaveChangesAsync();
            return deliveryPerson;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deliveryPerson = await _context.DeliveryPeople.FindAsync(id);
            if (deliveryPerson == null) return false;
            _context.DeliveryPeople.Remove(deliveryPerson);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
