
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Security.Claims;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Implement your authentication logic here
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader == null)
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));

        // var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
         var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
        var credentialBytes = Convert.FromBase64String(authHeaderValue.Parameter!); // Use null-forgiving operator
        // var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
        var username = credentials[0];
        var password = credentials[1];

        // Validate credentials (this is just an example, use a secure method in production)
        if (username == "user" && password == "password")
        {
            var claims = new[] { new Claim(ClaimTypes.Name, username) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        else
        {
            // return AuthenticateResult.Fail("Invalid Username or Password");
            return Task.FromResult(AuthenticateResult.Fail("Authentication failed."));
        }

    }
}