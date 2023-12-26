using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthanticateAndAuthorizeFlow.Controllers
{
    [Authorize(Roles = "Admin,Editor")]
    public class AdminController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
