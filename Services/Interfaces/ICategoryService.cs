using Delivery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.Services.Interfaces
{
    /// <summary>
    /// Interface para serviço de categorias
    /// </summary>
    public interface ICategoryService
    {
        Task<Category?> UpdateCategoryAsync(int id, Category category);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> AddCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
