using Delivery.Models;
using Microsoft.AspNetCore.Authorization;
using Delivery.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        [HttpPut("{id}")]
        public async Task<ActionResult<Review>> Update(int id, [FromBody] Review review)
        {
            try
            {
                var updated = await _reviewService.UpdateReviewAsync(id, review);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar avaliação: {ex.Message}");
            }
        }
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetAll()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpPost]
        public async Task<ActionResult<Review>> Add([FromBody] Delivery.Dtos.Review.ReviewDto dto)
        {
            var created = await _reviewService.AddReviewAsync(new Review {
                UserId = dto.UserId,
                EstablishmentId = dto.EstablishmentId,
                DeliveryPersonId = dto.DeliveryPersonId,
                OrderId = dto.OrderId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = dto.CreatedAt
            });
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _reviewService.DeleteReviewAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
