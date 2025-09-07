using Delivery.Models;

namespace Delivery.Repositories.Interfaces
{
    /// <summary>
    /// Interface para operações de repositório de pagamentos
    /// </summary>
    public interface IPaymentRepository
    {
        /// <summary>
        /// Retorna todos os pagamentos
        /// </summary>
        Task<IEnumerable<Payment>> GetAllAsync();

        /// <summary>
        /// Busca um pagamento pelo ID
        /// </summary>
        Task<Payment?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona um novo pagamento
        /// </summary>
        Task<Payment> AddAsync(Payment payment);

        /// <summary>
        /// Remove um pagamento pelo ID
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
