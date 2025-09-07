using Delivery.Models;

namespace Delivery.Repositories.Interfaces
{
    /// <summary>
    /// Interface para operações de repositório de avaliações
    /// </summary>
    public interface IReviewRepository
    {
        /// <summary>
        /// Retorna todas as avaliações
        /// </summary>
        Task<IEnumerable<Review>> GetAllAsync();

        /// <summary>
        /// Busca uma avaliação pelo ID
        /// </summary>
        Task<Review?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona uma nova avaliação
        /// </summary>
        Task<Review> AddAsync(Review review);

    /// <summary>
    /// Remove uma avaliação pelo ID
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Busca uma avaliação pelo usuário e pedido
    /// </summary>
    Task<Review?> FindByUserAndOrderAsync(int userId, int orderId);
    }
}
