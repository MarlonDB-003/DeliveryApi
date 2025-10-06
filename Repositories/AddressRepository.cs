using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Data;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        public async Task<Address?> UpdateAsync(int id, Address address)
        {
            var entity = await _context.Addresses.FindAsync(id);
            if (entity == null) return null;
            entity.UserId = address.UserId;
            entity.EstablishmentId = address.EstablishmentId;
            entity.Description = address.Description;
            entity.Street = address.Street;
            entity.Number = address.Number;
            entity.Complement = address.Complement;
            entity.Neighborhood = address.Neighborhood;
            entity.City = address.City;
            entity.State = address.State;
            entity.ZipCode = address.ZipCode;
            await _context.SaveChangesAsync();
            return entity;
        }
        private readonly DeliveryContext _context;
        public AddressRepository(DeliveryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _context.Addresses.ToListAsync();
        }

        public async Task<Address?> GetByIdAsync(int id)
        {
            return await _context.Addresses.FindAsync(id);
        }

        public async Task<Address> AddAsync(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return false;
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Address?> FindByUserAndDescriptionAsync(int userId, string description)
        {
            return await _context.Addresses.FirstOrDefaultAsync(a => a.UserId == userId && a.Description == description);
        }
    }
}
