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
            /// Autentica o usuário e retorna o token JWT
            /// </summary>
            string AuthenticateUserAndGenerateToken(Delivery.Controllers.UserLoginDto dto);
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
    /// Cadastra um novo usuário a partir do DTO, realizando todas as validações e regras de negócio
    /// </summary>
    Task<User> RegisterUserAsync(Delivery.Controllers.RegisterUserDto dto);

        /// <summary>
        /// Remove um usuário pelo ID
        /// </summary>
        Task<bool> DeleteUserAsync(int id);
        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        Task<User?> UpdateUserAsync(int id, Delivery.Dtos.User.UserDto dto);
    }
}
