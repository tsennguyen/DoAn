using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Laptop.Models;
using Shopping_Laptop.Repository;

namespace Shopping_Laptop.Controllers
{
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;

        public BrandController(DataContext context)
        {
            _dataContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> Index(string slug = "")
        {
            if (string.IsNullOrEmpty(slug))
            {
                return View(new List<ProductModel>());
            }

            var brand = await _dataContext.Brands
                .FirstOrDefaultAsync(c => c.Slug == slug);

            if (brand == null)
            {
                TempData["Error"] = "Thương hiệu không tồn tại hoặc đã bị xóa.";
                return View(new List<ProductModel>());
            }

            var productsByBrand = await _dataContext.Products
                .Where(p => p.BrandId == brand.Id)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return View(productsByBrand);
        }
    }
}
