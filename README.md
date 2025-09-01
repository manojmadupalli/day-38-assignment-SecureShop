
# SecureShop (ASP.NET Core MVC, .NET 8)

A secure-by-default demo: Identity (cookie auth), role-based authorization (Admin/Customer), EF Core (SQLite), input validation, output encoding, and basic rate limiting.

## Quick Start

```bash
# 1) Ensure .NET 8 SDK is installed
dotnet --info

# 2) Restore packages
dotnet restore

# 3) Build
dotnet build

# 4) Run (HTTPS)
dotnet run
```

Open: https://localhost:5001 (or the URL shown in the console)

### Default accounts (seeded)

- Admin: `admin@secureshop.test` / `Admin@123!`
- Customer: `customer@secureshop.test` / `Customer@123!`

> On first run the SQLite DB `app.db` is created automatically and roles, users, and products are seeded.

## Security Highlights

- **Authentication**: ASP.NET Core Identity (cookie-based). Secure logout uses the built-in sign-out endpoint.
- **Authorization**: Role-based. `Admin` can access `/Admin/Dashboard`. Customers can place orders (`/Orders/Create`).
- **Input Validation**: DataAnnotations on models and view models + jQuery unobtrusive validation on the client.
- **Output Encoding**: Razor encodes by default (no `@Html.Raw` used).
- **SQL Injection**: EF Core LINQ/parameterized queries; **no string-concatenated SQL**.
- **Brute-force Mitigation**: Identity lockout (5 failed attempts -> 15 min) + global rate limiting (60 req/min/IP).
- **Password Policy**: Min length=8, requires digit, uppercase, and special char.
- **CSRF**: Anti-forgery token on form posts (`[ValidateAntiForgeryToken]`).

## Project Structure

- `Program.cs` – configures services, Identity, RateLimiter, security headers; seeds data.
- `Data/ApplicationDbContext.cs` – EF Core context (Identity + domain entities).
- `Data/DbInitializer.cs` – Creates DB and seeds roles/users/products.
- `Models/` – `ApplicationUser`, `Product`, `Order`.
- `Controllers/` – `Home`, `Products`, `Orders`, `Admin`.
- `Views/` – Razor views. `_Layout` includes Bootstrap, validation scripts, and `_LoginPartial`.
- `wwwroot/` – CSS/JS.

## Switching to SQL Server (optional)

1. Add provider:
   ```xml
   <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.*" />
   ```
2. In `Program.cs`, replace `UseSqlite(...)` with:
   ```csharp
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
   ```
3. In `appsettings.json`, set a SQL Server connection string (e.g. `Server=.\SQLEXPRESS;Database=SecureShopDB;Trusted_Connection=True;TrustServerCertificate=True`).

> For production, prefer EF Core migrations. This demo uses `EnsureCreated()` for simplicity.

## Notes for the Assignment Mapping

- **User Story 1 (Validation/Encoding)**: DataAnnotations + client-side validation, Razor encoding by default.
- **User Story 2 (SQL Injection)**: Only LINQ via EF Core; no dynamic SQL.
- **User Story 3 (AuthZ)**: Roles `Admin` and `Customer`; `[Authorize]` attributes & UI restrictions.
- **User Story 4 (Registration/Login)**: Identity default UI. Passwords stored as salted hashes.
- **Bonus (Logout)**: Default Identity logout endpoint clears auth cookie and redirects.
