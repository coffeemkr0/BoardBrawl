using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace BoardBrawl.WebApp.MVC.Utils
{
    public class DebugAuthHandlerOptions : AuthenticationSchemeOptions
    {
    }

    public class DebugAuthHandler : AuthenticationHandler<DebugAuthHandlerOptions>
    {
        public const string AuthenticationScheme = "Debug";

        public DebugAuthHandler(IOptionsMonitor<DebugAuthHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var userId = Context.Request.Cookies["UserId"];
            if (userId == null)
            {
                userId = Guid.NewGuid().ToString();
                Context.Response.Cookies.Append("UserId", userId);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Name, "Debug user"),
                new(ClaimTypes.Email, "debug@example.com"),
                new(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
