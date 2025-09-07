
using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Delivery.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(IRestaurantRepository restaurantRepository, ILogger<RestaurantService> logger)
        {
            _restaurantRepository = restaurantRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            try
            {
                return await _restaurantRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar restaurantes.");
                throw new ApplicationException("Erro ao buscar restaurantes.");
            }
        }

        public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                return await _restaurantRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar restaurante {id}.");
                throw new ApplicationException("Erro ao buscar restaurante.");
            }
        }

        public async Task<Restaurant> AddRestaurantAsync(Restaurant restaurant)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(restaurant.Name))
                throw new ArgumentException("Nome do restaurante é obrigatório.");
            if (string.IsNullOrWhiteSpace(restaurant.Address))
                throw new ArgumentException("Endereço do restaurante é obrigatório.");

            try
            {
                var created = await _restaurantRepository.AddAsync(restaurant);
                _logger.LogInformation($"Restaurante criado: {created.Id} - {created.Name}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar restaurante.");
                throw new ApplicationException("Erro ao criar restaurante.");
            }
        }

        public async Task<bool> DeleteRestaurantAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                var result = await _restaurantRepository.DeleteAsync(id);
                if (result)
                    _logger.LogInformation($"Restaurante deletado: {id}");
                else
                    _logger.LogWarning($"Tentativa de deletar restaurante inexistente: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar restaurante {id}.");
                throw new ApplicationException("Erro ao deletar restaurante.");
            }
        }
    }
}
