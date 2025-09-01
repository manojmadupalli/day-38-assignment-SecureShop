
using Microsoft.AspNetCore.Mvc;

namespace SecureShop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Privacy() => View();
    }
}
