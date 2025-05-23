using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Laptop.Models;
using Shopping_Laptop.Repository;

namespace Shopping_Laptop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;

        public CategoryController(DataContext context)
        {
            _dataContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Hiển thị danh sách sản phẩm theo danh mục (Slug)
        public async Task<IActionResult> Index(string slug = "", string sortBy = "", decimal? startPrice = null, decimal? endPrice = null)
        {
            if (string.IsNullOrEmpty(slug))
            {
                TempData["Error"] = "Vui lòng chọn một danh mục.";
                return RedirectToAction("Index", "Home");
            }

            var category = await _dataContext.Categories
                .FirstOrDefaultAsync(c => c.Slug == slug);

            if (category == null)
            {
                TempData["Error"] = "Danh mục không tồn tại hoặc đã bị xóa.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Slug = slug;

            var productsByCategory = _dataContext.Products
                .Where(p => p.CategoryId == category.Id);

            var count = await productsByCategory.CountAsync();
            if (count == 0)
            {
                TempData["Info"] = "Không có sản phẩm nào trong danh mục này.";
                return View(new List<ProductModel>());
            }

            // Áp dụng sắp xếp
            productsByCategory = sortBy switch
            {
                "price_increase" => productsByCategory.OrderBy(p => p.Price),
                "price_decrease" => productsByCategory.OrderByDescending(p => p.Price),
                "price_newest" => productsByCategory.OrderByDescending(p => p.Id),
                "price_oldest" => productsByCategory.OrderBy(p => p.Id),
                _ => productsByCategory.OrderByDescending(p => p.Id)
            };

            // Lọc theo giá
            if (startPrice.HasValue && endPrice.HasValue)
            {
                productsByCategory = productsByCategory.Where(p => 
                    p.Price >= startPrice.Value && 
                    p.Price <= endPrice.Value);
            }

            var products = await productsByCategory.ToListAsync();
            return View(products);
        }
    }
}
