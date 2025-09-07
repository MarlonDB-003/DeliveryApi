using Delivery.Models;

namespace Delivery.Repositories.Interfaces
{
    /// <summary>
    /// Interface para operações de repositório de restaurantes
    /// </summary>
    public interface IRestaurantRepository
    {
        /// <summary>
        /// Retorna todos os restaurantes
        /// </summary>
        Task<IEnumerable<Restaurant>> GetAllAsync();

        /// <summary>
        /// Busca um restaurante pelo ID
        /// </summary>
        Task<Restaurant?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona um novo restaurante
        /// </summary>
        Task<Restaurant> AddAsync(Restaurant restaurant);

        /// <summary>
        /// Remove um restaurante pelo ID
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
