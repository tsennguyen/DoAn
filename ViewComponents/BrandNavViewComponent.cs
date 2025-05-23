using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Laptop.Repository; // Correct: For DataContext
using Shopping_Laptop.Models;    // Correct: For BrandModel
using System.Linq;
using System.Threading.Tasks;

namespace Shopping_Laptop.Components
{
    public class BrandNavViewComponent : ViewComponent
    {
        private readonly DataContext _dataContext; // Correct: Use DataContext

        public BrandNavViewComponent(DataContext context) // Correct: Inject DataContext
        {
            _dataContext = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var brands = await _dataContext.Brands.OrderBy(b => b.Name).ToListAsync();
            return View("Default", brands);
        }
    }
}