
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureShop.Data;
using SecureShop.Models;
using System.ComponentModel.DataAnnotations;

namespace SecureShop.Controllers
{
    [Authorize(Roles = $"{DbInitializer.CustomerRole},{DbInitializer.AdminRole}")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create(int productId)
        {
            var product = await _db.Products.FindAsync(productId);
            if (product is null) return NotFound();

            var vm = new CreateOrderVm { ProductId = product.Id, ProductName = product.Name, Price = product.Price };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.GetUserAsync(User);
            if (user is null) return Challenge();

            var product = await _db.Products.FindAsync(vm.ProductId);
            if (product is null) return NotFound();

            var order = new Order
            {
                ProductId = product.Id,
                Quantity = vm.Quantity,
                ShippingAddress = vm.ShippingAddress,
                UserId = user.Id,
                PlacedAt = DateTime.UtcNow
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            TempData["Toast"] = "Order placed successfully!";
            return RedirectToAction("Index", "Products");
        }

        public class CreateOrderVm
        {
            [Required]
            public int ProductId { get; set; }

            public string ProductName { get; set; } = string.Empty;

            public decimal Price { get; set; }

            [Range(1, 10, ErrorMessage = "Quantity must be between 1 and 10.")]
            public int Quantity { get; set; } = 1;

            [Required, StringLength(200, MinimumLength = 10)]
            public string ShippingAddress { get; set; } = string.Empty;
        }
    }
}
