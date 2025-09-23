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
            if (role != "cliente" && role != "estabelecimento" && role != "entregador" && role != "admin")
                return BadRequest("Role inválido. Use: cliente, estabelecimento, entregador ou admin.");

            // Validações específicas por tipo
            if (role == "estabelecimento")
            {
                if (string.IsNullOrWhiteSpace(dto.RestaurantName))
                    return BadRequest("O nome do restaurante é obrigatório.");
                if (dto.Address == null)
                    return BadRequest("O endereço é obrigatório.");
                var addr = dto.Address;
                if (string.IsNullOrWhiteSpace(addr.Street))
                    return BadRequest("O logradouro do endereço é obrigatório.");
                if (string.IsNullOrWhiteSpace(addr.Number))
                    return BadRequest("O número do endereço é obrigatório.");
                if (string.IsNullOrWhiteSpace(addr.Neighborhood))
                    return BadRequest("O bairro do endereço é obrigatório.");
                if (string.IsNullOrWhiteSpace(addr.City))
                    return BadRequest("A cidade do endereço é obrigatória.");
                if (string.IsNullOrWhiteSpace(addr.State))
                    return BadRequest("O estado do endereço é obrigatório.");
                if (string.IsNullOrWhiteSpace(addr.ZipCode))
                    return BadRequest("O CEP do endereço é obrigatório.");
                // Validação dos novos campos obrigatórios
                if (dto.MinimumOrderValue <= 0)
                    return BadRequest("O valor mínimo para pedido deve ser maior que zero.");
                if (dto.DeliveryFee < 0)
                    return BadRequest("A taxa de entrega não pode ser negativa.");
                // HasDeliveryPerson é bool, não precisa validar valor, mas pode validar presença se necessário
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

            object? estabelecimento = null;
            object? entregador = null;

            if (role == "estabelecimento")
            {
                var addr = dto.Address!;
                var addressString = $"{addr.Street}, {addr.Number}, {addr.Neighborhood}, {addr.City}";
                var est = new Establishment
                {
                    Name = dto.RestaurantName!,
                    Address = addressString,
                    Description = dto.Description ?? string.Empty,
                    ImageUrl = dto.ImageUrl ?? string.Empty,
                    OpeningTime = dto.OpeningTime ?? TimeSpan.Zero,
                    ClosingTime = dto.ClosingTime ?? TimeSpan.Zero,
                    CreatedAt = DateTime.UtcNow,
                    UserId = user.Id,
                    HasDeliveryPerson = dto.HasDeliveryPerson,
                    MinimumOrderValue = dto.MinimumOrderValue,
                    DeliveryFee = dto.DeliveryFee
                };
                _context.Establishments.Add(est);
                _context.SaveChanges();

                // Criar e salvar o endereço detalhado
                var addressEntity = new Address
                {
                    Description = addr.Description,
                    Street = addr.Street,
                    Number = addr.Number,
                    Neighborhood = addr.Neighborhood,
                    City = addr.City,
                    State = addr.State,
                    ZipCode = addr.ZipCode,
                    Complement = addr.Complement,
                    IsMain = true,
                    EstablishmentId = est.Id
                };
                _context.Addresses.Add(addressEntity);
                _context.SaveChanges();

                estabelecimento = new {
                    est.Id,
                    est.Name,
                    est.Address,
                    est.UserId,
                    est.Description,
                    est.ImageUrl,
                    est.OpeningTime,
                    est.ClosingTime,
                    est.HasDeliveryPerson,
                    est.MinimumOrderValue,
                    est.DeliveryFee,
                    AddressInfo = new {
                        addressEntity.Id,
                        addressEntity.Description,
                        addressEntity.Street,
                        addressEntity.Number,
                        addressEntity.Neighborhood,
                        addressEntity.City,
                        addressEntity.State,
                        addressEntity.ZipCode,
                        addressEntity.Complement,
                        addressEntity.IsMain
                    }
                };
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
                Estabelecimento = estabelecimento,
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


    // Dados específicos para estabelecimento
    public string? RestaurantName { get; set; }
    public string? RestaurantAddress { get; set; } // (deprecado, manter para compatibilidade)
    public Delivery.Dtos.Establishment.EstablishmentAddressDto? Address { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public TimeSpan? OpeningTime { get; set; }
    public TimeSpan? ClosingTime { get; set; }
    public bool HasDeliveryPerson { get; set; }
    public decimal MinimumOrderValue { get; set; }
    public decimal DeliveryFee { get; set; }

        // Dados específicos para entregador
        public string? DeliveryPhone { get; set; }
        public string? DeliveryVehicle { get; set; }
    }
}
