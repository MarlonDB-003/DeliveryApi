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
        /// Busca um pedido pelo ID
        /// </summary>
        Task<Order?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona um novo pedido
        /// </summary>
        Task<Order> AddAsync(Order order);

        /// <summary>
        /// Remove um pedido pelo ID
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
