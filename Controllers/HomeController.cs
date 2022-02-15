using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewKaratIk.Models;
using System.Diagnostics;

namespace NewKaratIk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

    }
}