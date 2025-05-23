using Microsoft.AspNetCore.Mvc;
using Shopping_Laptop.Services.Recommendation;
using System.Security.Claims;

namespace Shopping_Laptop.Controllers
{
    public class RecommendationController : Controller
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpPost]
        public async Task<IActionResult> RecordBehavior(int productId, Services.Recommendation.BehaviorType behaviorType)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            await _recommendationService.RecordUserBehaviorAsync(userId, productId, behaviorType);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetRecommendations(int count = 4)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var recommendations = await _recommendationService.GetRecommendedProductsAsync(userId, count);
            return Json(recommendations);
        }
    }
} 