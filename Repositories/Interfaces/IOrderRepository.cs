using Delivery.Models;

namespace Delivery.Repositories.Interfaces
{
    /// <summary>
    /// Interface para operações de repositório de pedidos
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Retorna todos os pedidos
        /// </summary>
        Task<IEnumerable<Order>> GetAllAsync();

        /// <summary>
        /// Busca um pedido pelo ID com relacionamentos
        /// </summary>
        Task<Order?> GetByIdAsync(int id);

        /// <summary>
        /// Busca pedidos por ID do usuário
        /// </summary>
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Busca pedidos por ID do estabelecimento
        /// </summary>
        Task<IEnumerable<Order>> GetByEstablishmentIdAsync(int establishmentId);

        /// <summary>
        /// Busca pedidos por ID do entregador
        /// </summary>
        Task<IEnumerable<Order>> GetByDeliveryPersonIdAsync(int deliveryPersonId);

        /// <summary>
        /// Adiciona um novo pedido
        /// </summary>
        Task<Order> AddAsync(Order order);

        /// <summary>
        /// Atualiza um pedido existente
        /// </summary>
        Task<Order> UpdateAsync(Order order);

        /// <summary>
        /// Remove um pedido pelo ID
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
