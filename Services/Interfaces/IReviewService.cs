using Delivery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.Services.Interfaces
{
    /// <summary>
    /// Interface para serviço de avaliações
    /// </summary>
    public interface IReviewService
    {
        Task<Review?> UpdateReviewAsync(int id, Review review);
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review?> GetReviewByIdAsync(int id);
        Task<Review> AddReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int id);
    }
}
