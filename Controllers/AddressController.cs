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
    public class AddressController : ControllerBase
    {
        [HttpPut("{id}")]
        public async Task<ActionResult<Address>> Update(int id, [FromBody] Address address)
        {
            try
            {
                var updated = await _addressService.UpdateAddressAsync(id, address);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar endereço: {ex.Message}");
            }
        }
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAll()
        {
            var addresses = await _addressService.GetAllAddressesAsync();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetById(int id)
        {
            var address = await _addressService.GetAddressByIdAsync(id);
            if (address == null) return NotFound();
            return Ok(address);
        }

        [HttpPost]
        public async Task<ActionResult<Address>> Add([FromBody] Delivery.Dtos.Address.AddressDto dto)
        {
            var created = await _addressService.AddAddressAsync(new Address {
                UserId = dto.UserId,
                Street = dto.Street,
                Number = dto.Number,
                Neighborhood = dto.Neighborhood,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
                Complement = dto.Complement,
                IsMain = dto.IsMain
            });
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _addressService.DeleteAddressAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
