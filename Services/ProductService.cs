
using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Delivery.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository productRepository, 
            IEstablishmentRepository establishmentRepository,
            ICategoryRepository categoryRepository,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _establishmentRepository = establishmentRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _productRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar produtos.");
                throw new ApplicationException("Erro ao buscar produtos.");
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                return await _productRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar produto {id}.");
                throw new ApplicationException("Erro ao buscar produto.");
            }
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Nome do produto é obrigatório.");
            if (product.Price <= 0)
                throw new ArgumentException("Preço do produto deve ser maior que zero.");

            // Regra de negócio: não permitir produto duplicado
            var existing = await _productRepository.FindByNameAsync(product.Name);
            if (existing != null)
                throw new InvalidOperationException("Produto com este nome já existe.");

            try
            {
                var created = await _productRepository.AddAsync(product);
                _logger.LogInformation($"Produto criado: {created.Id} - {created.Name}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar produto.");
                throw new ApplicationException("Erro ao criar produto.");
            }
        }

        public async Task<Product> CreateProductAsync(Delivery.Dtos.Product.ProductCreateDto dto, int userId)
        {
            // Validação dos campos obrigatórios
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("O nome do produto é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ArgumentException("A descrição do produto é obrigatória.");
            if (dto.Price <= 0)
                throw new ArgumentException("O preço do produto deve ser maior que zero.");
            if (dto.CategoryId <= 0)
                throw new ArgumentException("A categoria do produto é obrigatória.");

            try
            {
                // Busca o estabelecimento do usuário logado
                var establishment = await _establishmentRepository.GetByUserIdAsync(userId);
                if (establishment == null)
                    throw new InvalidOperationException("Usuário não possui estabelecimento cadastrado.");

                // Validação se a categoria existe
                var categoryExists = await _categoryRepository.GetByIdAsync(dto.CategoryId);
                if (categoryExists == null)
                    throw new ArgumentException("A categoria informada não existe.");

                // Verifica se já existe produto com o mesmo nome no estabelecimento
                var existingProduct = await _productRepository.FindByNameAndEstablishmentAsync(dto.Name, establishment.Id);
                if (existingProduct != null)
                    throw new InvalidOperationException("Já existe um produto com este nome neste estabelecimento.");

                // Cria o produto
                var product = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    ImageUrl = dto.ImageUrl,
                    CategoryId = dto.CategoryId,
                    EstablishmentId = establishment.Id,
                    CreatedAt = DateTime.UtcNow
                };

                var created = await _productRepository.AddAsync(product);
                _logger.LogInformation($"Produto criado: {created.Id} - {created.Name} para estabelecimento {establishment.Id}");
                return created;
            }
            catch (Exception ex) when (!(ex is ArgumentException || ex is InvalidOperationException))
            {
                _logger.LogError(ex, "Erro ao criar produto.");
                throw new ApplicationException("Erro ao criar produto.");
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                var result = await _productRepository.DeleteAsync(id);
                if (result)
                    _logger.LogInformation($"Produto deletado: {id}");
                else
                    _logger.LogWarning($"Tentativa de deletar produto inexistente: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar produto {id}.");
                throw new ApplicationException("Erro ao deletar produto.");
            }
        }
    }
}
