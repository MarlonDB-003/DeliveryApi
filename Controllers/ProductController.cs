using Delivery.Models;
using Microsoft.AspNetCore.Authorization;
using Delivery.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Dtos.Product;

namespace Delivery.Controllers
{
    [ApiController]
    [Authorize(Roles = "estabelecimento")]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            try
            {
                var updated = await _productService.UpdateProductAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar produto: {ex.Message}");
            }
        }
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Add(ProductCreateDto dto)
        {
            // Recupera o id do usuário logado via token JWT
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            if (!int.TryParse(userIdClaim.Value, out int userId))
                return BadRequest("Id do usuário inválido.");

            try
            {
                var created = await _productService.CreateProductAsync(dto, userId);
                var response = new ProductResponseDto
                {
                    Id = created.Id,
                    Name = created.Name,
                    Description = created.Description,
                    Price = created.Price,
                    ImageUrl = created.ImageUrl
                };
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cadastrar produto: {ex.Message}");
            }
        }
    [HttpPost("upload-image")]
    [RequestSizeLimit(10_000_000)] // Limite de 10MB
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Retorna o caminho relativo para uso futuro
            return Ok(new { fileName });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
