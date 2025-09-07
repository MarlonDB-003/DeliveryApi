
using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Delivery.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            try
            {
                return await _orderRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pedidos.");
                throw new ApplicationException("Erro ao buscar pedidos.");
            }
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                return await _orderRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar pedido {id}.");
                throw new ApplicationException("Erro ao buscar pedido.");
            }
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            // Validação básica
            if (order.UserId <= 0)
                throw new ArgumentException("Usuário do pedido é obrigatório.");
            if (order.RestaurantId <= 0)
                throw new ArgumentException("Restaurante do pedido é obrigatório.");
            if (order.OrderItems == null || order.OrderItems.Count == 0)
                throw new ArgumentException("Pedido deve conter ao menos um item.");

            // Regras de negócio podem ser expandidas aqui

            try
            {
                var created = await _orderRepository.AddAsync(order);
                _logger.LogInformation($"Pedido criado: {created.Id} para usuário {created.UserId}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pedido.");
                throw new ApplicationException("Erro ao criar pedido.");
            }
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                var result = await _orderRepository.DeleteAsync(id);
                if (result)
                    _logger.LogInformation($"Pedido deletado: {id}");
                else
                    _logger.LogWarning($"Tentativa de deletar pedido inexistente: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar pedido {id}.");
                throw new ApplicationException("Erro ao deletar pedido.");
            }
        }
    }
}
