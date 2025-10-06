using Delivery.Models;

namespace Delivery.Repositories.Interfaces
{
    /// <summary>
    /// Interface para operações de repositório de cupons
    /// </summary>
    public interface ICouponRepository
    {
        Task<Coupon?> UpdateAsync(int id, Coupon coupon);
        /// <summary>
        /// Retorna todos os cupons
        /// </summary>
        Task<IEnumerable<Coupon>> GetAllAsync();

        /// <summary>
        /// Busca um cupom pelo ID
        /// </summary>
        Task<Coupon?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona um novo cupom
        /// </summary>
        Task<Coupon> AddAsync(Coupon coupon);

    /// <summary>
    /// Remove um cupom pelo ID
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Busca um cupom pelo código
    /// </summary>
    Task<Coupon?> FindByCodeAsync(string code);
    }
}
