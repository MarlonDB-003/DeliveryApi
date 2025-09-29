
using Delivery.Models;
using Delivery.Repositories.Interfaces;
using Delivery.Dtos.Order;
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
        private readonly IProductRepository _productRepository;
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IEstablishmentRepository establishmentRepository,
            IUserRepository userRepository,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _establishmentRepository = establishmentRepository;
            _userRepository = userRepository;
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

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Id do usuário inválido.");

            try
            {
                return await _orderRepository.GetByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar pedidos do usuário {userId}.");
                throw new ApplicationException("Erro ao buscar pedidos do usuário.");
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByEstablishmentIdAsync(int establishmentId)
        {
            if (establishmentId <= 0)
                throw new ArgumentException("Id do estabelecimento inválido.");

            try
            {
                return await _orderRepository.GetByEstablishmentIdAsync(establishmentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar pedidos do estabelecimento {establishmentId}.");
                throw new ApplicationException("Erro ao buscar pedidos do estabelecimento.");
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByDeliveryPersonIdAsync(int deliveryPersonId)
        {
            if (deliveryPersonId <= 0)
                throw new ArgumentException("Id do entregador inválido.");

            try
            {
                return await _orderRepository.GetByDeliveryPersonIdAsync(deliveryPersonId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar pedidos do entregador {deliveryPersonId}.");
                throw new ApplicationException("Erro ao buscar pedidos do entregador.");
            }
        }

        public async Task<Order> CreateOrderAsync(OrderCreateDto orderDto, int userId)
        {
            // Validações básicas
            if (userId <= 0)
                throw new ArgumentException("Usuário inválido.");
            
            if (orderDto.EstablishmentId <= 0)
                throw new ArgumentException("Estabelecimento inválido.");
            
            if (orderDto.Items == null || !orderDto.Items.Any())
                throw new ArgumentException("O pedido deve conter pelo menos um item.");

            try
            {
                // Verificar se o usuário existe
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    throw new ArgumentException("Usuário não encontrado.");

                // Verificar se o estabelecimento existe
                var establishment = await _establishmentRepository.GetByIdAsync(orderDto.EstablishmentId);
                if (establishment == null)
                    throw new ArgumentException("Estabelecimento não encontrado.");

                // Calcular valores do pedido
                var subTotal = await CalculateSubTotalAsync(orderDto.Items);
                var deliveryFee = orderDto.DeliveryFee ?? CalculateDeliveryFee(subTotal);
                var totalAmount = subTotal + deliveryFee;

                // Criar o pedido
                var order = new Order
                {
                    UserId = userId,
                    EstablishmentId = orderDto.EstablishmentId,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Pendente",
                    SubTotal = subTotal,
                    DeliveryFee = deliveryFee,
                    TotalAmount = totalAmount,
                    DeliveryAddress = orderDto.DeliveryAddress,
                    ObservationsForEstablishment = orderDto.ObservationsForEstablishment,
                    ObservationsForDelivery = orderDto.ObservationsForDelivery,
                    OrderItems = new List<OrderItem>()
                };

                // Criar os itens do pedido
                foreach (var itemDto in orderDto.Items)
                {
                    var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                    if (product == null)
                        throw new ArgumentException($"Produto com ID {itemDto.ProductId} não encontrado.");

                    if (product.EstablishmentId != orderDto.EstablishmentId)
                        throw new ArgumentException($"O produto {product.Name} não pertence ao estabelecimento selecionado.");

                    var orderItem = new OrderItem
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        UnitPrice = product.Price,
                        Observations = itemDto.Observations
                    };

                    order.OrderItems.Add(orderItem);
                }

                var createdOrder = await _orderRepository.AddAsync(order);
                _logger.LogInformation($"Pedido criado: {createdOrder.Id} para usuário {userId}");
                
                return createdOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pedido.");
                throw;
            }
        }

        public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderUpdateStatusDto statusDto, int userId)
        {
            if (orderId <= 0)
                throw new ArgumentException("Id do pedido inválido.");

            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    throw new ArgumentException("Pedido não encontrado.");

                // Verificar se o usuário tem permissão para atualizar o status
                // (lógica de autorização pode ser expandida aqui)

                order.Status = statusDto.Status;
                order.DeliveryPersonId = statusDto.DeliveryPersonId;
                order.EstimatedDeliveryTime = statusDto.EstimatedDeliveryTime;
                order.StatusObservations = statusDto.StatusObservations;
                
                if (statusDto.Status == "Entregue")
                    order.DeliveredAt = DateTime.UtcNow;

                var updatedOrder = await _orderRepository.UpdateAsync(order);
                _logger.LogInformation($"Status do pedido {orderId} atualizado para {statusDto.Status}");
                
                return updatedOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar status do pedido {orderId}.");
                throw;
            }
        }

        public async Task<bool> CancelOrderAsync(int orderId, int userId)
        {
            if (orderId <= 0)
                throw new ArgumentException("Id do pedido inválido.");

            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    throw new ArgumentException("Pedido não encontrado.");

                // Verificar se o usuário pode cancelar o pedido
                if (order.UserId != userId)
                    throw new UnauthorizedAccessException("Usuário não autorizado a cancelar este pedido.");

                // Verificar se o pedido pode ser cancelado
                if (order.Status == "Entregue" || order.Status == "Cancelado")
                    throw new InvalidOperationException("Este pedido não pode ser cancelado.");

                order.Status = "Cancelado";
                order.UpdatedAt = DateTime.UtcNow;
                
                await _orderRepository.UpdateAsync(order);
                _logger.LogInformation($"Pedido {orderId} cancelado pelo usuário {userId}");
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao cancelar pedido {orderId}.");
                throw;
            }
        }

        public async Task<Order> AssignDeliveryPersonAsync(int orderId, int deliveryPersonId)
        {
            if (orderId <= 0)
                throw new ArgumentException("Id do pedido inválido.");

            if (deliveryPersonId <= 0)
                throw new ArgumentException("Id do entregador inválido.");

            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    throw new ArgumentException("Pedido não encontrado.");

                order.DeliveryPersonId = deliveryPersonId;
                order.Status = "ACaminho";
                order.UpdatedAt = DateTime.UtcNow;

                var updatedOrder = await _orderRepository.UpdateAsync(order);
                _logger.LogInformation($"Entregador {deliveryPersonId} atribuído ao pedido {orderId}");
                
                return updatedOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atribuir entregador ao pedido {orderId}.");
                throw;
            }
        }

        public async Task<decimal> CalculateOrderTotalAsync(List<OrderItemCreateDto> items)
        {
            decimal subTotal = await CalculateSubTotalAsync(items);
            decimal deliveryFee = CalculateDeliveryFee(subTotal);
            
            return subTotal + deliveryFee;
        }

        public async Task<decimal> CalculateSubTotalAsync(List<OrderItemCreateDto> items)
        {
            decimal subTotal = 0;

            foreach (var item in items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    subTotal += product.Price * item.Quantity;
                }
            }

            return subTotal;
        }

        public decimal CalculateDeliveryFee(decimal subTotal)
        {
            // Lógica para calcular taxa de entrega
            // Você pode personalizar conforme sua regra de negócio
            
            // Exemplo: Taxa fixa de R$ 5,00 para pedidos abaixo de R$ 30,00
            if (subTotal < 30.00m)
                return 5.00m;
            
            // Taxa gratuita para pedidos acima de R$ 30,00
            return 0.00m;
        }
    }
}
