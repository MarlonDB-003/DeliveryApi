using Delivery.Models;
using Microsoft.AspNetCore.Authorization;
using Delivery.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase // Categorias podem ser públicas para consulta
    {
        [HttpPut("{id}")]
        [Authorize(Roles = "admin,estabelecimento")] // Apenas admins e estabelecimentos podem editar categorias
        public async Task<ActionResult<Delivery.Dtos.Category.CategoryResponseDto>> Update(int id, [FromBody] Category category)
        {
            try
            {
                var updated = await _categoryService.UpdateCategoryAsync(id, category);
                if (updated == null) return NotFound();
                var dto = new Delivery.Dtos.Category.CategoryResponseDto {
                    Id = updated.Id,
                    Name = updated.Name,
                    Description = updated.Description
                };
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar categoria: {ex.Message}");
            }
        }
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var dtos = categories.Select(c => new Delivery.Dtos.Category.CategoryResponseDto {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            var dto = new Delivery.Dtos.Category.CategoryDetailResponseDto {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Add(Category category)
        {
            var created = await _categoryService.AddCategoryAsync(category);
            var dto = new Delivery.Dtos.Category.CategoryResponseDto {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description
            };
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _categoryService.DeleteCategoryAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
