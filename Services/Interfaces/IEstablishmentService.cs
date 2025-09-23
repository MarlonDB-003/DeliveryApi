using Delivery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.Services.Interfaces
{
    /// <summary>
    /// Interface para serviço de estabelecimentos
    /// </summary>
    public interface IEstablishmentService
    {
        Task<IEnumerable<Establishment>> GetAllEstablishmentsAsync();
        Task<Establishment?> GetEstablishmentByIdAsync(int id);
        Task<Establishment> AddEstablishmentAsync(Establishment establishment);
        Task<bool> DeleteEstablishmentAsync(int id);
    }
}
