using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Laptop.Models;
using Shopping_Laptop.Repository;
using Shopping_Laptop.Services.Recommendation;
using System.Security.Claims;

namespace Shopping_Laptop.ViewComponents
{
    public class RecommendedProductsViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        private readonly IRecommendationService _recommendationService;

        public RecommendedProductsViewComponent(DataContext context, IRecommendationService recommendationService)
        {
            _context = context;
            _recommendationService = recommendationService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count = 4)
        {
            var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            List<ProductModel> recommendedProducts;

            if (!string.IsNullOrEmpty(userId))
            {
                // Nếu user đã đăng nhập, lấy sản phẩm đề xuất dựa trên lịch sử
                var recommendedIds = await _recommendationService.GetRecommendedProductsAsync(userId, count);
                recommendedProducts = await _context.Products
                    .Where(p => recommendedIds.Contains(p.Id))
                    .Take(count)
                    .ToListAsync();

                // Nếu không đủ số lượng, bổ sung thêm sản phẩm phổ biến
                if (recommendedProducts.Count < count)
                {
                    var remainingCount = count - recommendedProducts.Count;
                    var existingIds = recommendedProducts.Select(p => p.Id).ToList();
                    var additionalProducts = await _context.Products
                        .Where(p => !existingIds.Contains(p.Id))
                        .OrderByDescending(p => p.Rating)
                        .ThenByDescending(p => p.Sold)
                        .Take(remainingCount)
                        .ToListAsync();

                    recommendedProducts.AddRange(additionalProducts);
                }
            }
            else
            {
                // Nếu user chưa đăng nhập, lấy sản phẩm phổ biến
                recommendedProducts = await _context.Products
                    .OrderByDescending(p => p.Rating)
                    .ThenByDescending(p => p.Sold)
                    .Take(count)
                    .ToListAsync();
            }

            return View(recommendedProducts);
        }
    }
} 