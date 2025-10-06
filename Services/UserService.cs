
using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;

namespace Delivery.Services
{
    public class UserService : IUserService
    {
        public async Task<User?> UpdateUserAsync(int id, Delivery.Dtos.User.UserDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("O nome do usuário é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("O e-mail do usuário é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("A senha do usuário é obrigatória.");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,
                Address = dto.Address,
                Phone = dto.Phone
            };

            try
            {
                var updated = await _userRepository.UpdateAsync(id, user);
                if (updated == null)
                    throw new InvalidOperationException("Usuário não encontrado.");
                _logger.LogInformation($"Usuário atualizado: {updated.Id} - {updated.FullName}");
                return updated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar usuário {id}.");
                throw new ApplicationException("Erro ao atualizar usuário.");
            }
        }
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public string AuthenticateUserAndGenerateToken(Delivery.Controllers.UserLoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Usuário e senha são obrigatórios.");

            var user = _userRepository.FindByEmailAsync(dto.Username).GetAwaiter().GetResult();
            if (user == null || !VerifyPassword(dto.Password, user.Password ?? ""))
                return string.Empty;

            // Migração automática: se a senha está em SHA256, atualiza para BCrypt
            if (!string.IsNullOrEmpty(user.Password) && IsLegacyPassword(user.Password))
            {
                try
                {
                    user.Password = HashPassword(dto.Password);
                    _userRepository.UpdateAsync(user.Id, user).Wait();
                    _logger.LogInformation($"Senha do usuário {user.Email} migrada para BCrypt");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"Falha ao migrar senha do usuário {user.Email}");
                }
            }

            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
                new System.Security.Claims.Claim("id", user.Id.ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.FullName ?? ""),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email ?? ""),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.Role ?? "")
            };

            var jwtKey = _configuration["Jwt:Key"];
            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey));
            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            var tokenString = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
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
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.FullName))
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

        public async Task<User> RegisterUserAsync(Delivery.Controllers.RegisterUserDto dto)
        {
            // Validação dos campos obrigatórios
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("O nome completo é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("O email é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Phone))
                throw new ArgumentException("O telefone é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("A senha é obrigatória.");
            if (string.IsNullOrWhiteSpace(dto.Type))
                throw new ArgumentException("O tipo de usuário é obrigatório (cliente ou estabelecimento).");

            // Validação de formato de email
            if (!dto.Email.Contains("@") || !dto.Email.Contains("."))
                throw new ArgumentException("O email informado é inválido.");

            // Validação de telefone (mínimo 8 dígitos)
            if (dto.Phone.Length < 8)
                throw new ArgumentException("O telefone deve conter pelo menos 8 dígitos.");

            // Validação de senha (mínimo 6 caracteres)
            if (dto.Password.Length < 6)
                throw new ArgumentException("A senha deve conter pelo menos 6 caracteres.");

            var existing = await _userRepository.FindByEmailAsync(dto.Email);
            if (existing != null)
                throw new InvalidOperationException("Email já cadastrado.");

            var type = dto.Type?.ToLower();
            if (type != "cliente" && type != "estabelecimento")
                throw new ArgumentException("Tipo inválido. Use: cliente ou estabelecimento.");

            var user = new User
            {
                FullName = dto.FullName!,
                Email = dto.Email!,
                Phone = dto.Phone!,
                Password = HashPassword(dto.Password!),
                Address = dto.Address,
                Role = type!,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                var created = await _userRepository.AddAsync(user);
                _logger.LogInformation($"Usuário cadastrado: {created.Id} - {created.Email}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar usuário.");
                throw new ApplicationException("Erro ao cadastrar usuário.");
            }
        }

        private static string HashPassword(string password)
        {
            // BCrypt com salt automático - muito mais seguro que SHA256
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        private static bool VerifyPassword(string password, string hash)
        {
            try
            {
                // Tenta verificar com BCrypt primeiro
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (Exception)
            {
                // Se falhar, tenta o método antigo SHA256 (para compatibilidade)
                return hash == HashPasswordLegacy(password);
            }
        }

        private static string HashPasswordLegacy(string password)
        {
            // Método antigo SHA256 para compatibilidade com senhas existentes
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private static bool IsLegacyPassword(string hash)
        {
            // Senhas SHA256 têm 64 caracteres hexadecimais
            // Senhas BCrypt começam com $2a$, $2b$, $2x$ ou $2y$
            return hash.Length == 64 && !hash.StartsWith("$2");
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
