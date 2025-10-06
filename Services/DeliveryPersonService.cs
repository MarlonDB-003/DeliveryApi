
using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Delivery.Services
{
    public class DeliveryPersonService : IDeliveryPersonService
    {
        public async Task<DeliveryPerson?> UpdateDeliveryPersonAsync(int id, DeliveryPerson deliveryPerson)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            if (string.IsNullOrWhiteSpace(deliveryPerson.Name))
                throw new ArgumentException("Nome do entregador é obrigatório.");
            if (string.IsNullOrWhiteSpace(deliveryPerson.Phone))
                throw new ArgumentException("Telefone do entregador é obrigatório.");
            try
            {
                var updated = await _deliveryPersonRepository.UpdateAsync(id, deliveryPerson);
                if (updated == null)
                    throw new InvalidOperationException("Entregador não encontrado.");
                _logger.LogInformation($"Entregador atualizado: {updated.Id} - {updated.Name}");
                return updated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar entregador {id}.");
                throw new ApplicationException("Erro ao atualizar entregador.");
            }
        }
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly ILogger<DeliveryPersonService> _logger;

        public DeliveryPersonService(IDeliveryPersonRepository deliveryPersonRepository, ILogger<DeliveryPersonService> logger)
        {
            _deliveryPersonRepository = deliveryPersonRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<DeliveryPerson>> GetAllDeliveryPeopleAsync()
        {
            try
            {
                return await _deliveryPersonRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar entregadores.");
                throw new ApplicationException("Erro ao buscar entregadores.");
            }
        }

        public async Task<DeliveryPerson?> GetDeliveryPersonByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                return await _deliveryPersonRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar entregador {id}.");
                throw new ApplicationException("Erro ao buscar entregador.");
            }
        }

        public async Task<DeliveryPerson> AddDeliveryPersonAsync(DeliveryPerson deliveryPerson)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(deliveryPerson.Name))
                throw new ArgumentException("Nome do entregador é obrigatório.");
            if (string.IsNullOrWhiteSpace(deliveryPerson.Phone))
                throw new ArgumentException("Telefone do entregador é obrigatório.");

            try
            {
                var created = await _deliveryPersonRepository.AddAsync(deliveryPerson);
                _logger.LogInformation($"Entregador criado: {created.Id} - {created.Name}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar entregador.");
                throw new ApplicationException("Erro ao criar entregador.");
            }
        }

        public async Task<bool> DeleteDeliveryPersonAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                var result = await _deliveryPersonRepository.DeleteAsync(id);
                if (result)
                    _logger.LogInformation($"Entregador deletado: {id}");
                else
                    _logger.LogWarning($"Tentativa de deletar entregador inexistente: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar entregador {id}.");
                throw new ApplicationException("Erro ao deletar entregador.");
            }
        }
    }
}
