
using Delivery.Models;
using Delivery.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Delivery.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IPaymentRepository paymentRepository, ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            try
            {
                return await _paymentRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pagamentos.");
                throw new ApplicationException("Erro ao buscar pagamentos.");
            }
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                return await _paymentRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar pagamento {id}.");
                throw new ApplicationException("Erro ao buscar pagamento.");
            }
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            // Validação básica
            if (payment.OrderId <= 0)
                throw new ArgumentException("Pedido do pagamento é obrigatório.");
            if (payment.Amount <= 0)
                throw new ArgumentException("Valor do pagamento deve ser maior que zero.");

            try
            {
                var created = await _paymentRepository.AddAsync(payment);
                _logger.LogInformation($"Pagamento criado: {created.Id} para pedido {created.OrderId}");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pagamento.");
                throw new ApplicationException("Erro ao criar pagamento.");
            }
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                var result = await _paymentRepository.DeleteAsync(id);
                if (result)
                    _logger.LogInformation($"Pagamento deletado: {id}");
                else
                    _logger.LogWarning($"Tentativa de deletar pagamento inexistente: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar pagamento {id}.");
                throw new ApplicationException("Erro ao deletar pagamento.");
            }
        }
    }
}
