using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Delivery.Services
{
    public class EstablishmentService : IEstablishmentService
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly ILogger<EstablishmentService> _logger;

        public EstablishmentService(IEstablishmentRepository establishmentRepository, ILogger<EstablishmentService> logger)
        {
            _establishmentRepository = establishmentRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Establishment>> GetAllEstablishmentsAsync()
        {
            try
            {
                return await _establishmentRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar estabelecimentos.");
                throw new ApplicationException("Erro ao buscar estabelecimentos.");
            }
        }

        public async Task<Establishment?> GetEstablishmentByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                return await _establishmentRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar estabelecimento {id}.");
                throw new ApplicationException("Erro ao buscar estabelecimento.");
            }
        }

        public async Task<Establishment> AddEstablishmentAsync(Establishment establishment)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(establishment.Name))
                throw new ArgumentException("Nome do estabelecimento é obrigatório.");
            if (string.IsNullOrWhiteSpace(establishment.Address))
                throw new ArgumentException("Endereço do estabelecimento é obrigatório.");

            try
            {
                var created = await _establishmentRepository.AddAsync(establishment);
                _logger.LogInformation($"Estabelecimento criado: {created.Id} - {created.Name}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar estabelecimento.");
                throw new ApplicationException("Erro ao criar estabelecimento.");
            }
        }

        public async Task<bool> DeleteEstablishmentAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                var result = await _establishmentRepository.DeleteAsync(id);
                if (result)
                    _logger.LogInformation($"Estabelecimento deletado: {id}");
                else
                    _logger.LogWarning($"Tentativa de deletar estabelecimento inexistente: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar estabelecimento {id}.");
                throw new ApplicationException("Erro ao deletar estabelecimento.");
            }
        }
    }
}
