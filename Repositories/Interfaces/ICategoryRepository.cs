using Delivery.Models;

namespace Delivery.Repositories.Interfaces
{
    /// <summary>
    /// Interface para operações de repositório de categorias
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Retorna todas as categorias
        /// </summary>
        Task<IEnumerable<Category>> GetAllAsync();

        /// <summary>
        /// Busca uma categoria pelo ID
        /// </summary>
        Task<Category?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona uma nova categoria
        /// </summary>
        Task<Category> AddAsync(Category category);

    /// <summary>
    /// Remove uma categoria pelo ID
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Busca uma categoria pelo nome
    /// </summary>
    Task<Category?> FindByNameAsync(string name);
    }
}
