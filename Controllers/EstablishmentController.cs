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
    public class EstablishmentController : ControllerBase
    {
        private readonly IEstablishmentService _establishmentService;
        public EstablishmentController(IEstablishmentService establishmentService)
        {
            _establishmentService = establishmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Establishment>>> GetAll()
        {
            var establishments = await _establishmentService.GetAllEstablishmentsAsync();
            return Ok(establishments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Establishment>> GetById(int id)
        {
            var establishment = await _establishmentService.GetEstablishmentByIdAsync(id);
            if (establishment == null) return NotFound();
            return Ok(establishment);
        }

        [HttpPost]
        public async Task<ActionResult<Establishment>> Add(Establishment establishment)
        {
            var created = await _establishmentService.AddEstablishmentAsync(establishment);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
