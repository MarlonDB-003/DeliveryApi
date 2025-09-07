using Delivery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.Services.Interfaces
{
    /// <summary>
    /// Interface para serviço de restaurantes
    /// </summary>
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        Task<Restaurant?> GetRestaurantByIdAsync(int id);
        Task<Restaurant> AddRestaurantAsync(Restaurant restaurant);
        Task<bool> DeleteRestaurantAsync(int id);
    }
}
