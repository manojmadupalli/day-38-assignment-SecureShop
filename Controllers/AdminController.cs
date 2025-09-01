
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureShop.Data;

namespace SecureShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Dashboard()
        {
            var model = new DashboardVm
            {
                ProductCount = await _db.Products.CountAsync(),
                OrderCount = await _db.Orders.CountAsync(),
                LatestOrders = await _db.Orders
                    .Include(o => o.Product)
                    .OrderByDescending(o => o.PlacedAt)
                    .Take(10)
                    .ToListAsync()
            };
            return View(model);
        }

        public class DashboardVm
        {
            public int ProductCount { get; set; }
            public int OrderCount { get; set; }
            public List<SecureShop.Models.Order> LatestOrders { get; set; } = new();
        }
    }
}
