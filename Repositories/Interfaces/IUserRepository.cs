using Delivery.Models;

namespace Delivery.Repositories.Interfaces
{
    /// <summary>
    /// Interface para operações de repositório de usuários
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retorna todos os usuários
        /// </summary>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Busca um usuário pelo ID
        /// </summary>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona um novo usuário
        /// </summary>
        Task<User> AddAsync(User user);

    /// <summary>
    /// Remove um usuário pelo ID
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Busca um usuário pelo e-mail
    /// </summary>
    Task<User?> FindByEmailAsync(string email);
        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        Task<User?> UpdateAsync(int id, User user);
    }
}
