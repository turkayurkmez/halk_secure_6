using AuthanticateAndAuthorizeFlow.Models;
using AuthanticateAndAuthorizeFlow.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthanticateAndAuthorizeFlow.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService userService;

        public UsersController(UserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Login(string? nerelereGidem)
        {
            ViewBag.Url = nerelereGidem;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? nerelereGidem)
        {
            if (ModelState.IsValid)
            {
                //kullanıcıyı doğrula ve eğer bilgiler doğru ise Cookie'ye ekle.
                //123456  -> A^+'!V!56 
                var user = userService.ValidateUser(loginViewModel.UserName, loginViewModel.Password);
                if (user != null)
                {
                    var claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Role,user.Role),

                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    if (!string.IsNullOrEmpty(nerelereGidem) && Url.IsLocalUrl(nerelereGidem))
                    {
                        return Redirect(nerelereGidem);
                    }
                    return Redirect("/");
                }
                ModelState.AddModelError("login", "Kullanıcı ya da şifre hatalı");

            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
