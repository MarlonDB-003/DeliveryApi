using Delivery.Models;
using Delivery.Dtos.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.Services.Interfaces
{
    /// <summary>
    /// Interface para serviço de pedidos
    /// </summary>
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetOrdersByEstablishmentIdAsync(int establishmentId);
        Task<IEnumerable<Order>> GetOrdersByDeliveryPersonIdAsync(int deliveryPersonId);
        Task<Order> CreateOrderAsync(OrderCreateDto orderDto, int userId);
        Task<Order> UpdateOrderStatusAsync(int orderId, OrderUpdateStatusDto statusDto, int userId);
        Task<bool> CancelOrderAsync(int orderId, int userId);
        Task<Order> AssignDeliveryPersonAsync(int orderId, int deliveryPersonId);
        Task<decimal> CalculateOrderTotalAsync(List<OrderItemCreateDto> items);
        Task<decimal> CalculateSubTotalAsync(List<OrderItemCreateDto> items);
        decimal CalculateDeliveryFee(decimal subTotal);
    }
}
