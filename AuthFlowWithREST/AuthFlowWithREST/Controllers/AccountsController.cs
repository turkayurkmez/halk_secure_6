using AuthFlowWithREST.Models;
using AuthFlowWithREST.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthFlowWithREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserService userService;

        public AccountsController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = userService.ValidateUser(loginModel.UserName, loginModel.Password);
                if (user != null)
                {
                    //1. Onay için kullanılacak, sunucu tarafında saklanacak bir cümle bul:
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aman-burasi-cok-onemli"));
                    var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var claims = new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.Role),

                    };

                    var token = new JwtSecurityToken(
                        issuer: "halkbank.server",
                        audience: "halkbank.mobile",
                        claims: claims,
                        notBefore: DateTime.Now,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: credential

                        );


                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }

                ModelState.AddModelError("login", "Hatalı giriş");


            }

            return BadRequest(ModelState);
        }
    }
}
