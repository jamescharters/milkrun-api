using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace MilkrunApi;

public class BasicAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private const string AuthenticationType = "Basic";

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorizationHeader = Request.Headers.Authorization.ToString();

        if (authorizationHeader.StartsWith(AuthenticationType, StringComparison.OrdinalIgnoreCase))
        {
            var token = authorizationHeader[$"{AuthenticationType} ".Length..].Trim();

            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split(":");

            // DEVNOTE: this is obviously a naive, insecure approach. However, the implementation of Basic Authentication is done as asked
            // in the task spec, using the fixed credentials supplied via email.
            if (credentials[0].Equals("test_user") && credentials[1].Equals("test_password"))
            {
                var claims = new[] { new Claim("name", credentials[0]), new Claim(ClaimTypes.Role, "Admin") };
                var identity = new ClaimsIdentity(claims, AuthenticationType);
                var claimsPrincipal = new ClaimsPrincipal(identity);

                return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
            }

            Response.StatusCode = StatusCodes.Status401Unauthorized;

            return AuthenticateResult.Fail("Invalid user credentials");
        }

        return AuthenticateResult.NoResult();
    }
}