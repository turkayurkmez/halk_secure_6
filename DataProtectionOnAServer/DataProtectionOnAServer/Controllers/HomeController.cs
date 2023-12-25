using DataProtectionOnAServer.Models;
using DataProtectionOnAServer.Security;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DataProtectionOnAServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            DataProtector dataProtector = new DataProtector("secret.txt");
            var length = dataProtector.EncryptData("Bu veri acayip gizli!!!");
            string value = dataProtector.DecryptData(length);
            ViewBag.Value = value;
            return View();
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