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
        private readonly IAddressService _addressService;
        private readonly ILogger<EstablishmentService> _logger;

        public EstablishmentService(
            IEstablishmentRepository establishmentRepository,
            IAddressService addressService,
            ILogger<EstablishmentService> logger)
        {
            _establishmentRepository = establishmentRepository;
            _addressService = addressService;
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
            // Validação básica dos novos campos
            if (string.IsNullOrWhiteSpace(establishment.EstablishmentName))
                throw new ArgumentException("Nome do estabelecimento é obrigatório.");
            if (establishment.CategoryId == null)
                throw new ArgumentException("Categoria do estabelecimento é obrigatória.");
            if (establishment.OpeningTime == default)
                throw new ArgumentException("Horário de abertura é obrigatório.");
            if (establishment.ClosingTime == default)
                throw new ArgumentException("Horário de fechamento é obrigatório.");
            if (establishment.MinimumOrderValue < 0)
                throw new ArgumentException("Valor mínimo para pedido não pode ser negativo.");
            if (establishment.DeliveryFee < 0)
                throw new ArgumentException("Taxa de entrega não pode ser negativa.");

            try
            {
                var created = await _establishmentRepository.AddAsync(establishment);
                _logger.LogInformation($"Estabelecimento criado: {created.Id} - {created.EstablishmentName}");
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
        /// <summary>
        /// Cadastra um estabelecimento e seu endereço completo
        /// </summary>
        public async Task<Establishment> RegisterEstablishmentAsync(Delivery.Dtos.Establishment.EstablishmentRegisterDto dto, int userId)
        {
            // Validação dos campos obrigatórios do estabelecimento
            if (string.IsNullOrWhiteSpace(dto.EstablishmentName))
                throw new ArgumentException("O nome do estabelecimento é obrigatório.");
            if (dto.CategoryId == null)
                throw new ArgumentException("A categoria é obrigatória.");
            if (dto.OpeningTime == default)
                throw new ArgumentException("O horário de abertura é obrigatório.");
            if (dto.ClosingTime == default)
                throw new ArgumentException("O horário de fechamento é obrigatório.");
            if (dto.Address == null)
                throw new ArgumentException("O endereço é obrigatório.");

            // Validação se a categoria existe
            var categoryExists = await _establishmentRepository.CategoryExistsAsync(dto.CategoryId.Value);
            if (!categoryExists)
                throw new ArgumentException("A categoria informada não existe.");

            // Validação dos campos obrigatórios do endereço
            var addr = dto.Address;
            if (string.IsNullOrWhiteSpace(addr.Street))
                throw new ArgumentException("O logradouro do endereço é obrigatório.");
            if (string.IsNullOrWhiteSpace(addr.Number))
                throw new ArgumentException("O número do endereço é obrigatório.");
            if (string.IsNullOrWhiteSpace(addr.Neighborhood))
                throw new ArgumentException("O bairro do endereço é obrigatório.");
            if (string.IsNullOrWhiteSpace(addr.City))
                throw new ArgumentException("A cidade do endereço é obrigatória.");
            if (string.IsNullOrWhiteSpace(addr.State))
                throw new ArgumentException("O estado do endereço é obrigatório.");
            if (string.IsNullOrWhiteSpace(addr.ZipCode))
                throw new ArgumentException("O CEP do endereço é obrigatório.");

            // Criação do endereço
            var address = new Address
            {
                Description = addr.Description,
                Street = addr.Street,
                Number = addr.Number,
                Neighborhood = addr.Neighborhood,
                City = addr.City,
                State = addr.State,
                ZipCode = addr.ZipCode,
                Complement = addr.Complement,
                IsMain = true,
                EstablishmentId = null, // será preenchido após salvar o estabelecimento
            };

            // Criação do estabelecimento
            var establishment = new Establishment
            {
                EstablishmentName = dto.EstablishmentName,
                Address = $"{address.Street}, {address.Number}, {address.Neighborhood}, {address.City}",
                CategoryId = dto.CategoryId,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                OpeningTime = dto.OpeningTime,
                ClosingTime = dto.ClosingTime,
                HasDeliveryPerson = dto.HasDeliveryPerson,
                MinimumOrderValue = dto.MinimumOrderValue,
                DeliveryFee = dto.DeliveryFee,
                CreatedAt = DateTime.UtcNow,
                Products = new List<Product>(),
                UserId = userId
            };

            // Salva o estabelecimento primeiro para obter o Id
            var createdEstablishment = await _establishmentRepository.AddAsync(establishment);

            // Associa o endereço ao estabelecimento salvo
            address.EstablishmentId = createdEstablishment.Id;
            var createdAddress = await _addressService.AddAddressAsync(address);

            // Atualiza o campo Address do estabelecimento para o objeto Address completo
            createdEstablishment.Address = $"{createdAddress.Street}, {createdAddress.Number}, {createdAddress.Neighborhood}, {createdAddress.City}";

            _logger.LogInformation($"Estabelecimento e endereço criados: {createdEstablishment.Id} - {createdEstablishment.EstablishmentName}");
            return createdEstablishment;
        }
    }
}
