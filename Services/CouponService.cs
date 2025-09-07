
using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Delivery.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ILogger<CouponService> _logger;

        public CouponService(ICouponRepository couponRepository, ILogger<CouponService> logger)
        {
            _couponRepository = couponRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Coupon>> GetAllCouponsAsync()
        {
            try
            {
                return await _couponRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cupons.");
                throw new ApplicationException("Erro ao buscar cupons.");
            }
        }

        public async Task<Coupon?> GetCouponByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                return await _couponRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar cupom {id}.");
                throw new ApplicationException("Erro ao buscar cupom.");
            }
        }

        public async Task<Coupon> AddCouponAsync(Coupon coupon)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(coupon.Code))
                throw new ArgumentException("Código do cupom é obrigatório.");
            if (coupon.Discount <= 0)
                throw new ArgumentException("Desconto do cupom deve ser maior que zero.");

            // Regra de negócio: não permitir cupom duplicado por código
            var existing = await _couponRepository.FindByCodeAsync(coupon.Code);
            if (existing != null)
                throw new InvalidOperationException("Cupom com este código já existe.");

            try
            {
                var created = await _couponRepository.AddAsync(coupon);
                _logger.LogInformation($"Cupom criado: {created.Id} - {created.Code}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar cupom.");
                throw new ApplicationException("Erro ao criar cupom.");
            }
        }

        public async Task<bool> DeleteCouponAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                var result = await _couponRepository.DeleteAsync(id);
                if (result)
                    _logger.LogInformation($"Cupom deletado: {id}");
                else
                    _logger.LogWarning($"Tentativa de deletar cupom inexistente: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar cupom {id}.");
                throw new ApplicationException("Erro ao deletar cupom.");
            }
        }
    }
}
