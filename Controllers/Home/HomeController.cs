using Microsoft.AspNetCore.Mvc;
using Lab4.Data;

namespace Lab4.Controllers.Home
{
    public class HomeController : Controller
    {

        private readonly MarketDbContext _context;

        public HomeController(MarketDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
