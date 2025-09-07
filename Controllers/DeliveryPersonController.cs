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
        public async Task<ActionResult<DeliveryPerson>> Add(DeliveryPerson deliveryPerson)
        {
            var created = await _deliveryPersonService.AddDeliveryPersonAsync(deliveryPerson);
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
