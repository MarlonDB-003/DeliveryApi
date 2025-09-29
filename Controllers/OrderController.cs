using Delivery.Models;
using Delivery.Dtos.Order;
using Microsoft.AspNetCore.Authorization;
using Delivery.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Delivery.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Buscar todos os pedidos (apenas admin)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Buscar pedido por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null) 
                    return NotFound("Pedido não encontrado.");

                // Verificar se o usuário tem permissão para ver este pedido
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                
                if (userRole != "admin" && order.UserId != userId && order.EstablishmentId != GetUserEstablishmentId() && order.DeliveryPersonId != GetUserDeliveryPersonId())
                {
                    return Forbid("Você não tem permissão para acessar este pedido.");
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Buscar pedidos do usuário logado
        /// </summary>
        [HttpGet("my-orders")]
        [Authorize(Roles = "cliente")]
        public async Task<ActionResult<IEnumerable<Order>>> GetMyOrders()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var orders = await _orderService.GetOrdersByUserIdAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Buscar pedidos do estabelecimento
        /// </summary>
        [HttpGet("establishment/{establishmentId}")]
        [Authorize(Roles = "estabelecimento,admin")]
        public async Task<ActionResult<IEnumerable<Order>>> GetEstablishmentOrders(int establishmentId)
        {
            try
            {
                // Verificar se o usuário é dono do estabelecimento (implementar lógica conforme necessário)
                var orders = await _orderService.GetOrdersByEstablishmentIdAsync(establishmentId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Buscar pedidos do entregador
        /// </summary>
        [HttpGet("delivery-person/{deliveryPersonId}")]
        [Authorize(Roles = "entregador,admin")]
        public async Task<ActionResult<IEnumerable<Order>>> GetDeliveryPersonOrders(int deliveryPersonId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByDeliveryPersonIdAsync(deliveryPersonId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Criar um novo pedido
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "cliente")]
        public async Task<ActionResult<Order>> CreateOrder(OrderCreateDto orderDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var order = await _orderService.CreateOrderAsync(orderDto, userId);
                return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        /// <summary>
        /// Atualizar status do pedido
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "estabelecimento,entregador,admin")]
        public async Task<ActionResult<Order>> UpdateOrderStatus(int id, OrderUpdateStatusDto statusDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var order = await _orderService.UpdateOrderStatusAsync(id, statusDto, userId);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        /// <summary>
        /// Cancelar pedido
        /// </summary>
        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "cliente,admin")]
        public async Task<ActionResult> CancelOrder(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var success = await _orderService.CancelOrderAsync(id, userId);
                
                if (success)
                    return Ok(new { message = "Pedido cancelado com sucesso." });
                
                return BadRequest("Não foi possível cancelar o pedido.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        /// <summary>
        /// Atribuir entregador ao pedido
        /// </summary>
        [HttpPut("{id}/assign-delivery")]
        [Authorize(Roles = "estabelecimento,admin")]
        public async Task<ActionResult<Order>> AssignDeliveryPerson(int id, [FromBody] int deliveryPersonId)
        {
            try
            {
                var order = await _orderService.AssignDeliveryPersonAsync(id, deliveryPersonId);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        // Métodos auxiliares (implementar conforme a estrutura do seu projeto)
        private int GetUserEstablishmentId()
        {
            // Implementar lógica para buscar o ID do estabelecimento do usuário logado
            // Por enquanto, retorna 0
            return 0;
        }

        private int GetUserDeliveryPersonId()
        {
            // Implementar lógica para buscar o ID do entregador do usuário logado
            // Por enquanto, retorna 0
            return 0;
        }
    }
}
