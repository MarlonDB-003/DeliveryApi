using Delivery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.Services.Interfaces
{
    /// <summary>
    /// Interface para serviço de endereços
    /// </summary>
    public interface IAddressService
    {
        Task<Address?> UpdateAddressAsync(int id, Address address);
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task<Address?> GetAddressByIdAsync(int id);
        Task<Address> AddAddressAsync(Address address);
        Task<bool> DeleteAddressAsync(int id);
    }
}
