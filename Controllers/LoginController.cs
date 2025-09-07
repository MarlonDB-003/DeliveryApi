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
    public class LoginController : ControllerBase
    {
        private readonly Delivery.Data.DeliveryContext _context;

        public LoginController(Delivery.Data.DeliveryContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest("Email e senha são obrigatórios.");

            var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Username);
            if (user == null)
                return Unauthorized("Usuário não encontrado.");

            var passwordHash = HashPassword(loginDto.Password);
            if (user.PasswordHash != passwordHash)
                return Unauthorized("Senha inválida.");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email ?? ""),
                new Claim(ClaimTypes.Role, user.Role ?? "cliente")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKey@345SuperSecretKey@345SuperSecretKey@345!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "DeliveryApi",
                audience: "DeliveryApiUsers",
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = tokenString });
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
    }
}
