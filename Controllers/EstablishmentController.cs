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
    [Authorize]
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
                Name = e.Name,
                Description = e.Description,
                ImageUrl = e.ImageUrl,
                Address = e.Address,
                CategoryId = e.CategoryId,
                OpeningTime = e.OpeningTime,
                ClosingTime = e.ClosingTime,
                HasDeliveryPerson = e.HasDeliveryPerson,
                MinimumOrderValue = e.MinimumOrderValue,
                DeliveryFee = e.DeliveryFee,
                Email = e.Email
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
                Name = establishment.Name,
                Description = establishment.Description,
                ImageUrl = establishment.ImageUrl,
                Address = establishment.Address,
                CategoryId = establishment.CategoryId,
                OpeningTime = establishment.OpeningTime,
                ClosingTime = establishment.ClosingTime,
                HasDeliveryPerson = establishment.HasDeliveryPerson,
                MinimumOrderValue = establishment.MinimumOrderValue,
                DeliveryFee = establishment.DeliveryFee,
                Email = establishment.Email
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<EstablishmentDetailResponseDto>> Add(EstablishmentRegisterDto dto)
        {
            // Validação dos campos obrigatórios do estabelecimento
            if (string.IsNullOrWhiteSpace(dto.RestaurantName))
                return BadRequest("O nome do restaurante é obrigatório.");
            if (dto.CategoryId == null)
                return BadRequest("A categoria é obrigatória.");
            if (dto.OpeningTime == default)
                return BadRequest("O horário de abertura é obrigatório.");
            if (dto.ClosingTime == default)
                return BadRequest("O horário de fechamento é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("O email é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("A senha é obrigatória.");
            if (dto.Address == null)
                return BadRequest("O endereço é obrigatório.");

            // Validação dos campos obrigatórios do endereço
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

            // Criação do usuário
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = dto.Password, // Ideal: aplicar hash seguro
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            // Criação do endereço
            Address address = new Address
            {
                Description = addr.Description,
                Street = addr.Street,
                Number = addr.Number,
                Neighborhood = addr.Neighborhood,
                City = addr.City,
                State = addr.State,
                ZipCode = addr.ZipCode,
                Complement = addr.Complement,
                IsMain = true // Endereço principal do estabelecimento
            };

            // Criação do estabelecimento
            var establishment = new Establishment
            {
                Name = dto.RestaurantName,
                Address = $"{address.Street}, {address.Number}, {address.Neighborhood}, {address.City}",
                CategoryId = dto.CategoryId,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                OpeningTime = dto.OpeningTime,
                ClosingTime = dto.ClosingTime,
                HasDeliveryPerson = dto.HasDeliveryPerson,
                MinimumOrderValue = dto.MinimumOrderValue,
                DeliveryFee = dto.DeliveryFee,
                Email = dto.Email,
                PasswordHash = dto.Password, // Ideal: aplicar hash seguro
                CreatedAt = DateTime.UtcNow,
                User = user,
                Products = new List<Product>()
            };

            address.Establishment = establishment;

            var created = await _establishmentService.AddEstablishmentAsync(establishment);

            var dtoResponse = new EstablishmentDetailResponseDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                ImageUrl = created.ImageUrl,
                Address = created.Address,
                CategoryId = created.CategoryId,
                OpeningTime = created.OpeningTime,
                ClosingTime = created.ClosingTime,
                HasDeliveryPerson = created.HasDeliveryPerson,
                MinimumOrderValue = created.MinimumOrderValue,
                DeliveryFee = created.DeliveryFee,
                Email = created.Email
            };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, dtoResponse);
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
