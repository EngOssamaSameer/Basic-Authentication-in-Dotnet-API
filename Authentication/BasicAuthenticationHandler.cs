using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace BasicAuthentication.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.NoResult());

            var authHeader = Request.Headers["Authorization"].ToString();

            if(!authHeader.StartsWith("Basic",StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(AuthenticateResult.Fail("Unkown Scheme"));

            var encodeCred = authHeader["Basic".Length..];
            var decodeCred = Encoding.UTF8.GetString(Convert.FromBase64String(encodeCred));

            var cred = decodeCred.Split(":");

            var role = "Admin";
            if (cred[0] == "Nada")
            {
                role = "User";
            }

            // crate idenetity with claims
            var identity = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Role,role),
                new Claim(ClaimTypes.Name,cred[0])
            }, "Basic");

            // principal
            var principal = new ClaimsPrincipal(identity);

            // Create Teckit
            var teckit = new AuthenticationTicket(principal,"Basic");


            return Task.FromResult(AuthenticateResult.Success(teckit));
        }
    }
}
