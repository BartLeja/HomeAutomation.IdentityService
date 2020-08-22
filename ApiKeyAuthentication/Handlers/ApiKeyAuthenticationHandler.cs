using HomeAutomation.IdentityService.ApiKeyAuthentication.Options;
using HomeAutomation.IdentityService.ApiKeyAuthentication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace HomeAutomation.IdentityService.ApiKeyAuthentication.Handlers
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private const string ProblemDetailsContentType = "application/problem+json";
        private readonly IApiKeyService _apiKeyService;
        private const string ApiKeyHeaderName = "X-Api-Key";
        private const string HomeAutomationLocalLightSystemId = "Home-Automation-Local-LightSystem-Id";

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IApiKeyService apiKeyService) : base(options, logger, encoder, clock)
        {
            _apiKeyService = apiKeyService ?? throw new ArgumentNullException(nameof(apiKeyService));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
            {
                return AuthenticateResult.NoResult();
            }

            if (!Request.Headers.TryGetValue(HomeAutomationLocalLightSystemId, out var homeAutomationLocalLightSystemId))
            {
                return AuthenticateResult.NoResult();
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            var clientId = homeAutomationLocalLightSystemId.FirstOrDefault();

            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
            {
                return AuthenticateResult.NoResult();
            }

            var existingApiKey = await _apiKeyService.Execute(providedApiKey);

            if (existingApiKey != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingApiKey.Owner as string),
                    new Claim(ClaimTypes.PrimarySid, clientId as string)
                };

                claims.AddRange(existingApiKey.Roles.Select(role => new Claim(ClaimTypes.Role, role as string)));

                var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
                var identities = new List<ClaimsIdentity> { identity };
                var principal = new ClaimsPrincipal(identities);
                var ticket = new AuthenticationTicket(principal, Options.Scheme);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Invalid API Key provided.");
        }

        //protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        //{
        //    Response.StatusCode = 401;
        //    Response.ContentType = ProblemDetailsContentType;
        //    var problemDetails = new UnauthorizedProblemDetails();

        //    await Response.WriteAsync(JsonSerializer.Serialize(problemDetails, DefaultJsonSerializerOptions.Options));
        //}

        //protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        //{
        //    Response.StatusCode = 403;
        //    Response.ContentType = ProblemDetailsContentType;
        //    var problemDetails = new ForbiddenProblemDetails();

        //    await Response.WriteAsync(JsonSerializer.Serialize(problemDetails, DefaultJsonSerializerOptions.Options));
        //}
    }
}
