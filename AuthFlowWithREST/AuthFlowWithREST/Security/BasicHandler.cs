using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace AuthFlowWithREST.Security
{
    public class BasicHandler : AuthenticationHandler<BasicOption>
    {
        public BasicHandler(IOptionsMonitor<BasicOption> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            /*
             * Gelen http request'in header'ı içinde
             *   1. Authorization olacak.
             *   2. Authorization değeri doğru formatta olacak.
             *   2. Bu Authorization değeri  Basic olacak.
             *   3. Bu değerin karşılığında Encode edilmiş username:password olacak.
             *   
             *       Authorization: 'Basic ==apedlfrkgtıgtjfdy'
             */

            //1. Gelen talepte, Authorization var mı?
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            //2. Bu Authorization değeri doğru formatta mı?
            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out AuthenticationHeaderValue headerValue))
            {
                return Task.FromResult(AuthenticateResult.NoResult());

            }

            //Bu Authorization değeri  Basic olacak.
            if (!headerValue.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.NoResult());

            }


            // Bu değerin karşılığında Encode edilmiş username:password olacak.
            byte[] bytes = Convert.FromBase64String(headerValue.Parameter);
            string headerParameter = Encoding.UTF8.GetString(bytes);

            var userName = headerParameter.Split(':')[0];
            var password = headerParameter.Split(':')[1];

            if (userName == "turkay" && password == "123456")
            {
                var claims = new Claim[] { new Claim(ClaimTypes.Name, userName) };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                AuthenticationTicket ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));

            }

            return Task.FromResult(AuthenticateResult.Fail("Hatalı giriş"));



        }
    }
}
