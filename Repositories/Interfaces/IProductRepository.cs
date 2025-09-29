using Delivery.Models;

namespace Delivery.Repositories.Interfaces
{
    /// <summary>
    /// Interface para operações de repositório de produtos
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Retorna todos os produtos
        /// </summary>
        Task<IEnumerable<Product>> GetAllAsync();

        /// <summary>
        /// Busca um produto pelo ID
        /// </summary>
        Task<Product?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona um novo produto
        /// </summary>
        Task<Product> AddAsync(Product product);

    /// <summary>
    /// Remove um produto pelo ID
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Busca um produto pelo nome
    /// </summary>
    Task<Product?> FindByNameAsync(string name);

    /// <summary>
    /// Busca um produto pelo nome e estabelecimento
    /// </summary>
    Task<Product?> FindByNameAndEstablishmentAsync(string name, int establishmentId);
    }
}
