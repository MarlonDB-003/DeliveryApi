using Delivery.Models;
using Microsoft.AspNetCore.Authorization;
using Delivery.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(int id, [FromBody] Delivery.Dtos.User.UserDto dto)
        {
            try
            {
                var updated = await _userService.UpdateUserAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar usuário: {ex.Message}");
            }
        }
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous] // Permite cadastro público de usuários
        public async Task<ActionResult<User>> Add([FromBody] Delivery.Dtos.User.UserDto dto)
        {
            var created = await _userService.AddUserAsync(new User {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Password = dto.Password,
                Address = dto.Address,
                Role = dto.Type
            });
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
