using Delivery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.Services.Interfaces
{
    /// <summary>
    /// Interface para serviço de usuários
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retorna todos os usuários
        /// </summary>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// Busca um usuário pelo ID
        /// </summary>
        Task<User?> GetUserByIdAsync(int id);

        /// <summary>
        /// Adiciona um novo usuário
        /// </summary>
        Task<User> AddUserAsync(User user);

        /// <summary>
        /// Remove um usuário pelo ID
        /// </summary>
        Task<bool> DeleteUserAsync(int id);
    }
}
