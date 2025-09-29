using Delivery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Delivery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Delivery.Services.Interfaces.IUserService _userService;

        public AuthController(Delivery.Services.Interfaces.IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto dto)
        {
            try
            {
                var token = _userService.AuthenticateUserAndGenerateToken(dto);
                if (string.IsNullOrEmpty(token))
                    return Unauthorized("Usuário ou senha inválidos.");
                return Ok(new { token });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao autenticar: {ex.Message}");
            }
        }

        // Método utilitário para gerar hash SHA256 da senha
        private static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        [HttpPost("register")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public IActionResult Register([FromBody] RegisterUserDto dto)
        {
            try
            {
                var user = _userService.RegisterUserAsync(dto).GetAwaiter().GetResult();
                string? nextStep = null;
                if (user.Role == "estabelecimento")
                {
                    nextStep = "Finalize o cadastro do estabelecimento enviando os dados em /api/establishment";
                }
                return Created(string.Empty, new
                {
                    success = true,
                    message = "Usuário cadastrado com sucesso.",
                    user = new {
                        user.Id,
                        user.Name,
                        user.Email,
                        user.Phone,
                        user.Role
                    },
                    proximoPasso = nextStep
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cadastrar usuário: {ex.Message}");
            }
        }
    }

    public class UserLoginDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class RegisterUserDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? Type { get; set; }
    }
}
