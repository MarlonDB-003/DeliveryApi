
using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Delivery.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuários.");
                throw new ApplicationException("Erro ao buscar usuários.");
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                return await _userRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar usuário {id}.");
                throw new ApplicationException("Erro ao buscar usuário.");
            }
        }

        public async Task<User> AddUserAsync(User user)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(user.Name))
                throw new ArgumentException("Nome do usuário é obrigatório.");
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("E-mail do usuário é obrigatório.");

            // Regra de negócio: não permitir usuário duplicado por e-mail
            var existing = await _userRepository.FindByEmailAsync(user.Email);
            if (existing != null)
                throw new InvalidOperationException("Usuário com este e-mail já existe.");

            try
            {
                var created = await _userRepository.AddAsync(user);
                _logger.LogInformation($"Usuário criado: {created.Id} - {created.Email}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar usuário.");
                throw new ApplicationException("Erro ao criar usuário.");
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                var result = await _userRepository.DeleteAsync(id);
                if (result)
                    _logger.LogInformation($"Usuário deletado: {id}");
                else
                    _logger.LogWarning($"Tentativa de deletar usuário inexistente: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar usuário {id}.");
                throw new ApplicationException("Erro ao deletar usuário.");
            }
        }
    }
}
