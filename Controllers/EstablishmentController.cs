using Delivery.Models;
using Microsoft.AspNetCore.Authorization;
using Delivery.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Dtos.Establishment;

namespace Delivery.Controllers
{
        [ApiController]
        [Authorize(Roles = "estabelecimento")]
        [Route("api/[controller]")]
    public class EstablishmentController : ControllerBase
    {
        private readonly IEstablishmentService _establishmentService;
        public EstablishmentController(IEstablishmentService establishmentService)
        {
            _establishmentService = establishmentService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstablishmentDetailResponseDto>>> GetAll()
        {
            var establishments = await _establishmentService.GetAllEstablishmentsAsync();
            var dtos = establishments.Select(e => new EstablishmentDetailResponseDto
            {
                Id = e.Id,
                Name = e.EstablishmentName,
                Description = e.Description,
                ImageUrl = e.ImageUrl,
                Address = e.Address,
                CategoryId = e.CategoryId,
                OpeningTime = e.OpeningTime,
                ClosingTime = e.ClosingTime,
                HasDeliveryPerson = e.HasDeliveryPerson,
                MinimumOrderValue = e.MinimumOrderValue,
                DeliveryFee = e.DeliveryFee,
            }).ToList();
            return Ok(dtos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<EstablishmentDetailResponseDto>> GetById(int id)
        {
            var establishment = await _establishmentService.GetEstablishmentByIdAsync(id);
            if (establishment == null) return NotFound();
            var dto = new EstablishmentDetailResponseDto
            {
                Id = establishment.Id,
                Name = establishment.EstablishmentName,
                Description = establishment.Description,
                ImageUrl = establishment.ImageUrl,
                Address = establishment.Address,
                CategoryId = establishment.CategoryId,
                OpeningTime = establishment.OpeningTime,
                ClosingTime = establishment.ClosingTime,
                HasDeliveryPerson = establishment.HasDeliveryPerson,
                MinimumOrderValue = establishment.MinimumOrderValue,
                DeliveryFee = establishment.DeliveryFee,
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<EstablishmentDetailResponseDto>> Add(EstablishmentRegisterDto dto)
        {
            // Recupera o id do usuário logado via token JWT
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            if (!int.TryParse(userIdClaim.Value, out int userId))
                return BadRequest("Id do usuário inválido.");

            try
            {
                var created = await _establishmentService.RegisterEstablishmentAsync(dto, userId);
                var dtoResponse = new EstablishmentDetailResponseDto
                {
                    Id = created.Id,
                    Name = created.EstablishmentName,
                    Description = created.Description,
                    ImageUrl = created.ImageUrl,
                    Address = created.Address,
                    CategoryId = created.CategoryId,
                    OpeningTime = created.OpeningTime,
                    ClosingTime = created.ClosingTime,
                    HasDeliveryPerson = created.HasDeliveryPerson,
                    MinimumOrderValue = created.MinimumOrderValue,
                    DeliveryFee = created.DeliveryFee
                };
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, dtoResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cadastrar estabelecimento: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _establishmentService.DeleteEstablishmentAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
