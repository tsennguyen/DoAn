using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Laptop.Models;
using Shopping_Laptop.Repository;
using Shopping_Laptop.Services.Recommendation;
using System.Security.Claims;

namespace Shopping_Laptop.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IRecommendationService _recommendationService;

        public ProductController(DataContext context, IRecommendationService recommendationService)
        {
            _dataContext = context;
            _recommendationService = recommendationService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index", "Home");
            }

            var products = await _dataContext.Products
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .ToListAsync();

            ViewBag.Keyword = searchTerm;

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                await _recommendationService.RecordUserBehaviorAsync(userId, id, Services.Recommendation.BehaviorType.Clicked);
            }

            var product = await _dataContext.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var relatedProducts = await _dataContext.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                .Take(4)
                .ToListAsync();

            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommentProduct(RatingModel rating)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["error"] = "Vui lòng đăng nhập để đánh giá sản phẩm";
                    return RedirectToAction("Details", new { id = rating.ProductId });
                }

                rating.UserId = userId;
                rating.CreatedAt = DateTime.Now;

                _dataContext.Ratings.Add(rating);
                await _dataContext.SaveChangesAsync();
                await UpdateProductRating(rating.ProductId);

                TempData["success"] = "Thêm đánh giá thành công";

                var referer = Request.Headers["Referer"].ToString();
                return !string.IsNullOrEmpty(referer) ? Redirect(referer) : RedirectToAction("Details", new { id = rating.ProductId });
            }

            TempData["error"] = "Vui lòng điền đầy đủ thông tin đánh giá";
            return RedirectToAction("Details", new { id = rating.ProductId });
        }

        private async Task UpdateProductRating(int productId)
        {
            var product = await _dataContext.Products
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product?.Ratings != null && product.Ratings.Any())
            {
                product.Rating = (decimal)product.Ratings.Average(r => r.Star);
                await _dataContext.SaveChangesAsync();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRating(int productId, int rating, string comment)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để đánh giá sản phẩm" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Không thể xác định người dùng" });
            }

            var existingRating = await _dataContext.Ratings
                .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);

            if (existingRating != null)
            {
                existingRating.Star = rating;
                existingRating.Comment = comment;
                existingRating.CreatedAt = DateTime.Now;
            }
            else
            {
                var newRating = new RatingModel
                {
                    ProductId = productId,
                    UserId = userId,
                    Star = rating,
                    Comment = comment,
                    CreatedAt = DateTime.Now
                };
                _dataContext.Ratings.Add(newRating);
            }

            await _dataContext.SaveChangesAsync();
            await UpdateProductRating(productId);

            return Json(new { success = true, message = "Cảm ơn bạn đã đánh giá sản phẩm" });
        }
    }
}