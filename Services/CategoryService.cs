
using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Delivery.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _categoryRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar categorias.");
                throw new ApplicationException("Erro ao buscar categorias.");
            }
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                return await _categoryRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar categoria {id}.");
                throw new ApplicationException("Erro ao buscar categoria.");
            }
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Nome da categoria é obrigatório.");

            // Regra de negócio: não permitir categoria duplicada por nome
            var existing = await _categoryRepository.FindByNameAsync(category.Name);
            if (existing != null)
                throw new InvalidOperationException("Categoria com este nome já existe.");

            try
            {
                var created = await _categoryRepository.AddAsync(category);
                _logger.LogInformation($"Categoria criada: {created.Id} - {created.Name}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar categoria.");
                throw new ApplicationException("Erro ao criar categoria.");
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                var result = await _categoryRepository.DeleteAsync(id);
                if (result)
                    _logger.LogInformation($"Categoria deletada: {id}");
                else
                    _logger.LogWarning($"Tentativa de deletar categoria inexistente: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar categoria {id}.");
                throw new ApplicationException("Erro ao deletar categoria.");
            }
        }
    }
}
