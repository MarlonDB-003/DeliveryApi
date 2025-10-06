
using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Delivery.Services
{
    public class ReviewService : IReviewService
    {
        public async Task<Review?> UpdateReviewAsync(int id, Review review)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            if (review.UserId <= 0)
                throw new ArgumentException("Usuário da avaliação é obrigatório.");
            if (!review.OrderId.HasValue || review.OrderId.Value <= 0)
                throw new ArgumentException("Pedido da avaliação é obrigatório.");
            if (review.Rating < 1 || review.Rating > 5)
                throw new ArgumentException("Nota da avaliação deve ser entre 1 e 5.");
            try
            {
                var updated = await _reviewRepository.UpdateAsync(id, review);
                if (updated == null)
                    throw new InvalidOperationException("Avaliação não encontrada.");
                _logger.LogInformation($"Avaliação atualizada: {updated.Id}");
                return updated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar avaliação {id}.");
                throw new ApplicationException("Erro ao atualizar avaliação.");
            }
        }

        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IReviewRepository reviewRepository, ILogger<ReviewService> logger)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            try
            {
                return await _reviewRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar avaliações.");
                throw new ApplicationException("Erro ao buscar avaliações.");
            }
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                return await _reviewRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar avaliação {id}.");
                throw new ApplicationException("Erro ao buscar avaliação.");
            }
        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            // Validação básica

            if (review.UserId <= 0)
                throw new ArgumentException("Usuário da avaliação é obrigatório.");
            if (!review.OrderId.HasValue || review.OrderId.Value <= 0)
                throw new ArgumentException("Pedido da avaliação é obrigatório.");
            if (review.Rating < 1 || review.Rating > 5)
                throw new ArgumentException("Nota da avaliação deve ser entre 1 e 5.");

            // Regra de negócio: não permitir review duplicado por usuário e pedido
            var existing = await _reviewRepository.FindByUserAndOrderAsync(review.UserId, review.OrderId.Value);
            if (existing != null)
                throw new InvalidOperationException("Usuário já avaliou este pedido.");

            try
            {
                var created = await _reviewRepository.AddAsync(review);
                _logger.LogInformation($"Avaliação criada: {created.Id} para pedido {created.OrderId}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar avaliação.");
                throw new ApplicationException("Erro ao criar avaliação.");
            }
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                var result = await _reviewRepository.DeleteAsync(id);
                if (result)
                    _logger.LogInformation($"Avaliação deletada: {id}");
                else
                    _logger.LogWarning($"Tentativa de deletar avaliação inexistente: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar avaliação {id}.");
                throw new ApplicationException("Erro ao deletar avaliação.");
            }
        }
    }
}
