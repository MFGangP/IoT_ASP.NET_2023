using ASPNET02_WebApp.Data;
using ASPNET02_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASPNET02_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _db;
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // SELECT * FROM portfolios WHERE Division = 'PORTFOLIO';
            var model = _db.Portfolios.Where(q => q.Division == "PORTFOLIO"); 

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}