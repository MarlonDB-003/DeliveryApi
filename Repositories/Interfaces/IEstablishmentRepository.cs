using Delivery.Models;

namespace Delivery.Repositories.Interfaces
{
    /// <summary>
    /// Interface para operações de repositório de estabelecimentos
    /// </summary>
    public interface IEstablishmentRepository
    {
        /// <summary>
        /// Retorna todos os estabelecimentos
        /// </summary>
        Task<IEnumerable<Establishment>> GetAllAsync();

        /// <summary>
        /// Busca um estabelecimento pelo ID
        /// </summary>
        Task<Establishment?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona um novo estabelecimento
        /// </summary>
        Task<Establishment> AddAsync(Establishment establishment);

        /// <summary>
        /// Remove um estabelecimento pelo ID
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
