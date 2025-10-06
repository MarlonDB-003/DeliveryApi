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
    public class OrderItemController : ControllerBase
    {
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderItem>> Update(int id, [FromBody] OrderItem item)
        {
            try
            {
                var updated = await _orderItemService.UpdateOrderItemAsync(id, item);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar item do pedido: {ex.Message}");
            }
        }
        private readonly IOrderItemService _orderItemService;
        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetAll()
        {
            var items = await _orderItemService.GetAllOrderItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetById(int id)
        {
            var item = await _orderItemService.GetOrderItemByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<OrderItem>> Add(OrderItem item)
        {
            var created = await _orderItemService.AddOrderItemAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _orderItemService.DeleteOrderItemAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
