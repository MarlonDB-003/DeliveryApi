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
            Address? address = null;
            if (dto.Address != null)
            {
                address = new Address
                {
                    Description = dto.Address.Description,
                    Street = dto.Address.Street,
                    Number = dto.Address.Number,
                    Neighborhood = dto.Address.Neighborhood,
                    City = dto.Address.City,
                    State = dto.Address.State,
                    ZipCode = dto.Address.ZipCode,
                    Complement = dto.Address.Complement,
                    IsMain = true // Endereço principal do estabelecimento
                };
            }

            // Criação do estabelecimento
            var establishment = new Establishment
            {
                Name = dto.RestaurantName,
                Address = address != null ? $"{address.Street}, {address.Number}, {address.Neighborhood}, {address.City}" : null,
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

            if (address != null)
            {
                establishment.Products = new List<Product>();
                address.Establishment = establishment;
                establishment.Address = address.Street + ", " + address.Number + ", " + address.Neighborhood + ", " + address.City;
                // O Address será salvo em cascade se configurado no contexto, senão, salvar manualmente depois
                if (establishment.Products == null)
                    establishment.Products = new List<Product>();
                if (establishment.User == null)
                    establishment.User = user;
                if (establishment.Category == null && dto.CategoryId != null)
                    establishment.CategoryId = dto.CategoryId;
            }

            var created = await _establishmentService.AddEstablishmentAsync(establishment);

            // Se Address não for salvo em cascade, salvar manualmente
            // (Implementação depende do contexto e repositório)

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
