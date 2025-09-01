
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureShop.Data;

namespace SecureShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db) => _db = db;

        // Everyone can view
        public async Task<IActionResult> Index(string? q)
        {
            // Parameterized by LINQ -> prevents SQL injection
            var products = _db.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                products = products.Where(p => p.Name.Contains(term) || p.Description.Contains(term));
            }
            return View(await products.OrderBy(p => p.Name).ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
