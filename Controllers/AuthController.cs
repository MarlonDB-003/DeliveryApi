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
        // ...existing code...
        private readonly Delivery.Data.DeliveryContext _context;

        public AuthController(Delivery.Data.DeliveryContext context)
        {
            _context = context;
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
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Nome, email e senha são obrigatórios.");

            if (_context.Users.Any(u => u.Email == dto.Email))
                return Conflict("Email já cadastrado.");

            var role = dto.Role?.ToLower() ?? "cliente";
            if (role != "cliente" && role != "restaurante" && role != "entregador" && role != "admin")
                return BadRequest("Role inválido. Use: cliente, restaurante, entregador ou admin.");

            // Validações específicas por tipo
            if (role == "restaurante")
            {
                if (string.IsNullOrWhiteSpace(dto.RestaurantName) || string.IsNullOrWhiteSpace(dto.RestaurantAddress))
                    return BadRequest("Restaurante: nome e endereço são obrigatórios.");
                // Aqui você pode criar o registro do restaurante e vincular ao usuário
            }
            if (role == "entregador")
            {
                if (string.IsNullOrWhiteSpace(dto.DeliveryPhone) || string.IsNullOrWhiteSpace(dto.DeliveryVehicle))
                    return BadRequest("Entregador: telefone e veículo são obrigatórios.");
                // Aqui você pode criar o registro do entregador e vincular ao usuário
            }

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                Role = role,
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            object? restaurante = null;
            object? entregador = null;

            if (role == "restaurante")
            {
                var rest = new Restaurant
                {
                    Name = dto.RestaurantName!,
                    Address = dto.RestaurantAddress!,
                    Description = "",
                    ImageUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UserId = user.Id
                };
                _context.Restaurants.Add(rest);
                _context.SaveChanges();
                restaurante = new { rest.Id, rest.Name, rest.Address, rest.UserId };
            }
            if (role == "entregador")
            {
                var entreg = new DeliveryPerson
                {
                    Name = dto.Name!,
                    Email = dto.Email!,
                    Phone = dto.DeliveryPhone!,
                    Vehicle = dto.DeliveryVehicle!,
                    CreatedAt = DateTime.UtcNow,
                    Status = "ativo",
                    UserId = user.Id
                };
                _context.DeliveryPeople.Add(entreg);
                _context.SaveChanges();
                entregador = new { entreg.Id, entreg.Name, entreg.Phone, entreg.Vehicle, entreg.UserId };
            }

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Role,
                Restaurante = restaurante,
                Entregador = entregador
            });
            // ...existing code...
        }
    }

    public class UserLoginDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class RegisterUserDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }

        // Dados específicos para restaurante
        public string? RestaurantName { get; set; }
        public string? RestaurantAddress { get; set; }

        // Dados específicos para entregador
        public string? DeliveryPhone { get; set; }
        public string? DeliveryVehicle { get; set; }
    }
}
