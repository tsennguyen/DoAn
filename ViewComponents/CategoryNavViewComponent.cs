using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Laptop.Repository; // For DataContext
using Shopping_Laptop.Models;    // For CategoryModel
using System.Linq;
using System.Threading.Tasks;

namespace Shopping_Laptop.Components // Ensure this namespace is correct
{
    public class CategoryNavViewComponent : ViewComponent
    {
        private readonly DataContext _dataContext; // Changed to DataContext

        public CategoryNavViewComponent(DataContext context) // Changed to DataContext
        {
            _dataContext = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() // Changed to InvokeAsync and returns Task
        {
            var categories = await _dataContext.Categories
                                             .OrderBy(c => c.Name) // Optional: Order categories by name
                                             .ToListAsync(); // Asynchronous call
            return View("Default", categories); // Explicitly state view name "Default"
        }
    }
}