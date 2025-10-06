
using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Delivery.Services
{
    public class OrderItemService : IOrderItemService
    {
        public async Task<OrderItem?> UpdateOrderItemAsync(int id, OrderItem item)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            if (item.OrderId <= 0)
                throw new ArgumentException("Pedido do item é obrigatório.");
            if (item.ProductId <= 0)
                throw new ArgumentException("Produto do item é obrigatório.");
            if (item.Quantity <= 0)
                throw new ArgumentException("Quantidade do item deve ser maior que zero.");
            try
            {
                var updated = await _orderItemRepository.UpdateAsync(id, item);
                if (updated == null)
                    throw new InvalidOperationException("Item do pedido não encontrado.");
                _logger.LogInformation($"Item do pedido atualizado: {updated.Id}");
                return updated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar item do pedido {id}.");
                throw new ApplicationException("Erro ao atualizar item do pedido.");
            }
        }
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ILogger<OrderItemService> _logger;

        public OrderItemService(IOrderItemRepository orderItemRepository, ILogger<OrderItemService> logger)
        {
            _orderItemRepository = orderItemRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
        {
            try
            {
                return await _orderItemRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar itens do pedido.");
                throw new ApplicationException("Erro ao buscar itens do pedido.");
            }
        }

        public async Task<OrderItem?> GetOrderItemByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                return await _orderItemRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar item do pedido {id}.");
                throw new ApplicationException("Erro ao buscar item do pedido.");
            }
        }

        public async Task<OrderItem> AddOrderItemAsync(OrderItem item)
        {
            // Validação básica
            if (item.OrderId <= 0)
                throw new ArgumentException("Pedido do item é obrigatório.");
            if (item.ProductId <= 0)
                throw new ArgumentException("Produto do item é obrigatório.");
            if (item.Quantity <= 0)
                throw new ArgumentException("Quantidade do item deve ser maior que zero.");

            try
            {
                var created = await _orderItemRepository.AddAsync(item);
                _logger.LogInformation($"Item do pedido criado: {created.Id} para pedido {created.OrderId}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar item do pedido.");
                throw new ApplicationException("Erro ao criar item do pedido.");
            }
        }

        public async Task<bool> DeleteOrderItemAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                var result = await _orderItemRepository.DeleteAsync(id);
                if (result)
                    _logger.LogInformation($"Item do pedido deletado: {id}");
                else
                    _logger.LogWarning($"Tentativa de deletar item do pedido inexistente: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar item do pedido {id}.");
                throw new ApplicationException("Erro ao deletar item do pedido.");
            }
        }
    }
}
