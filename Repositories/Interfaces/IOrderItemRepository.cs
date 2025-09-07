using Delivery.Models;

namespace Delivery.Repositories.Interfaces
{
    /// <summary>
    /// Interface para operações de repositório de itens do pedido
    /// </summary>
    public interface IOrderItemRepository
    {
        /// <summary>
        /// Retorna todos os itens do pedido
        /// </summary>
        Task<IEnumerable<OrderItem>> GetAllAsync();

        /// <summary>
        /// Busca um item do pedido pelo ID
        /// </summary>
        Task<OrderItem?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona um novo item ao pedido
        /// </summary>
        Task<OrderItem> AddAsync(OrderItem item);

        /// <summary>
        /// Remove um item do pedido pelo ID
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
