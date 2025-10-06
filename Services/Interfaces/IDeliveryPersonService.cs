using Delivery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.Services.Interfaces
{
    /// <summary>
    /// Interface para serviço de entregadores
    /// </summary>
    public interface IDeliveryPersonService
    {
        Task<DeliveryPerson?> UpdateDeliveryPersonAsync(int id, DeliveryPerson deliveryPerson);
        Task<IEnumerable<DeliveryPerson>> GetAllDeliveryPeopleAsync();
        Task<DeliveryPerson?> GetDeliveryPersonByIdAsync(int id);
        Task<DeliveryPerson> AddDeliveryPersonAsync(DeliveryPerson deliveryPerson);
        Task<bool> DeleteDeliveryPersonAsync(int id);
    }
}
