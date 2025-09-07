using Delivery.Models;

namespace Delivery.Repositories.Interfaces
{
    /// <summary>
    /// Interface para operações de repositório de endereços
    /// </summary>
    public interface IAddressRepository
    {
        /// <summary>
        /// Retorna todos os endereços
        /// </summary>
        Task<IEnumerable<Address>> GetAllAsync();

        /// <summary>
        /// Busca um endereço pelo ID
        /// </summary>
        Task<Address?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona um novo endereço
        /// </summary>
        Task<Address> AddAsync(Address address);

    /// <summary>
    /// Remove um endereço pelo ID
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Busca um endereço pelo usuário e descrição
    /// </summary>
    Task<Address?> FindByUserAndDescriptionAsync(int userId, string description);
    }
}
