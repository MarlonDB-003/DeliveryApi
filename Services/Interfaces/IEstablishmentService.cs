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

    /// <summary>
    /// Cadastra um estabelecimento e seu endereço completo
    /// </summary>
    Task<Establishment> RegisterEstablishmentAsync(Delivery.Dtos.Establishment.EstablishmentRegisterDto dto, int userId);
        /// <summary>
        /// Atualiza um estabelecimento existente
        /// </summary>
        Task<Establishment?> UpdateEstablishmentAsync(int id, Delivery.Dtos.Establishment.EstablishmentDto dto);
    }
}
