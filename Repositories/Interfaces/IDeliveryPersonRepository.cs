using Delivery.Models;

namespace Delivery.Repositories.Interfaces
{
    /// <summary>
    /// Interface para operações de repositório de entregadores
    /// </summary>
    public interface IDeliveryPersonRepository
    {
        Task<DeliveryPerson?> UpdateAsync(int id, DeliveryPerson deliveryPerson);
        /// <summary>
        /// Retorna todos os entregadores
        /// </summary>
        Task<IEnumerable<DeliveryPerson>> GetAllAsync();

        /// <summary>
        /// Busca um entregador pelo ID
        /// </summary>
        Task<DeliveryPerson?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona um novo entregador
        /// </summary>
        Task<DeliveryPerson> AddAsync(DeliveryPerson deliveryPerson);

        /// <summary>
        /// Remove um entregador pelo ID
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
