
using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Delivery.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ILogger<AddressService> _logger;

        public AddressService(IAddressRepository addressRepository, ILogger<AddressService> logger)
        {
            _addressRepository = addressRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            try
            {
                return await _addressRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar endereços.");
                throw new ApplicationException("Erro ao buscar endereços.");
            }
        }

        public async Task<Address?> GetAddressByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                return await _addressRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar endereço {id}.");
                throw new ApplicationException("Erro ao buscar endereço.");
            }
        }

        public async Task<Address> AddAddressAsync(Address address)
        {

            // Validação básica
            if ((address.UserId == null || address.UserId <= 0) && (address.EstablishmentId == null || address.EstablishmentId <= 0))
                throw new ArgumentException("Usuário ou estabelecimento do endereço é obrigatório.");
            if (string.IsNullOrWhiteSpace(address.Description))
                throw new ArgumentException("Descrição do endereço é obrigatória.");

            // Regra de negócio: não permitir endereço duplicado para o mesmo usuário e descrição
            if (address.UserId != null && address.UserId > 0)
            {
                var existing = await _addressRepository.FindByUserAndDescriptionAsync(address.UserId.Value, address.Description);
                if (existing != null)
                    throw new InvalidOperationException("Endereço já cadastrado para este usuário.");
            }

            try
            {
                var created = await _addressRepository.AddAsync(address);
                _logger.LogInformation($"Endereço criado: {created.Id} para usuário {created.UserId}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar endereço.");
                throw new ApplicationException("Erro ao criar endereço.");
            }
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                var result = await _addressRepository.DeleteAsync(id);
                if (result)
                    _logger.LogInformation($"Endereço deletado: {id}");
                else
                    _logger.LogWarning($"Tentativa de deletar endereço inexistente: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar endereço {id}.");
                throw new ApplicationException("Erro ao deletar endereço.");
            }
        }
    }
}
