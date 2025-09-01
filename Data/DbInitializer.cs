
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecureShop.Models;

namespace SecureShop.Data
{
    public static class DbInitializer
    {
        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";

        public static async Task InitializeAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create schema
            await ctx.Database.EnsureCreatedAsync();

            // Roles
            foreach (var role in new[] { AdminRole, CustomerRole })
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed admin
            var adminEmail = "admin@secureshop.test";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "Site Administrator"
                };
                var result = await userManager.CreateAsync(admin, "Admin@123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, AdminRole);
            }

            // Seed a demo customer
            var custEmail = "customer@secureshop.test";
            var cust = await userManager.FindByEmailAsync(custEmail);
            if (cust is null)
            {
                cust = new ApplicationUser
                {
                    UserName = custEmail,
                    Email = custEmail,
                    EmailConfirmed = true,
                    FullName = "Demo Customer"
                };
                var result = await userManager.CreateAsync(cust, "Customer@123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(cust, CustomerRole);
            }

            // Seed products if empty
            if (!await ctx.Products.AnyAsync())
            {
                ctx.Products.AddRange(
                    new Product { Name = "Wireless Headphones", Description = "Noise-cancelling over-ear headphones.", Price = 149.99m, ImageUrl = "https://picsum.photos/seed/headphones/400/300" },
                    new Product { Name = "Smartwatch", Description = "Fitness tracking with heart-rate monitor.", Price = 199.99m, ImageUrl = "https://picsum.photos/seed/watch/400/300" },
                    new Product { Name = "Bluetooth Speaker", Description = "Portable speaker with deep bass.", Price = 89.50m, ImageUrl = "https://picsum.photos/seed/speaker/400/300" }
                );
                await ctx.SaveChangesAsync();
            }
        }
    }
}
