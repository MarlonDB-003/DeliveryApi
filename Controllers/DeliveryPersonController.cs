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
    public class DeliveryPersonController : ControllerBase
    {
        [HttpPut("{id}")]
        public async Task<ActionResult<DeliveryPerson>> Update(int id, [FromBody] DeliveryPerson deliveryPerson)
        {
            try
            {
                var updated = await _deliveryPersonService.UpdateDeliveryPersonAsync(id, deliveryPerson);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar entregador: {ex.Message}");
            }
        }
        private readonly IDeliveryPersonService _deliveryPersonService;
        public DeliveryPersonController(IDeliveryPersonService deliveryPersonService)
        {
            _deliveryPersonService = deliveryPersonService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryPerson>>> GetAll()
        {
            var deliveryPeople = await _deliveryPersonService.GetAllDeliveryPeopleAsync();
            return Ok(deliveryPeople);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryPerson>> GetById(int id)
        {
            var deliveryPerson = await _deliveryPersonService.GetDeliveryPersonByIdAsync(id);
            if (deliveryPerson == null) return NotFound();
            return Ok(deliveryPerson);
        }

        [HttpPost]
        public async Task<ActionResult<DeliveryPerson>> Add([FromBody] Delivery.Dtos.Delivery.DeliveryPersonDto dto)
        {
            var created = await _deliveryPersonService.AddDeliveryPersonAsync(new DeliveryPerson {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Vehicle = dto.Vehicle,
                ImageUrl = dto.ImageUrl
            });
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _deliveryPersonService.DeleteDeliveryPersonAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
