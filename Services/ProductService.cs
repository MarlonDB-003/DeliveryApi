
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
        private readonly ILogger<ProductService> _logger;
        private IProductRepository @object;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public ProductService(IProductRepository @object)
        {
            this.@object = @object;
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
