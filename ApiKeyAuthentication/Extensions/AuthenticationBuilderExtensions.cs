using HomeAutomation.IdentityService.ApiKeyAuthentication.Handlers;
using HomeAutomation.IdentityService.ApiKeyAuthentication.Options;
using Microsoft.AspNetCore.Authentication;
using System;

namespace HomeAutomation.IdentityService.ApiKeyAuthentication.Extensions
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKeySupport(
            this AuthenticationBuilder authenticationBuilder, 
            Action<ApiKeyAuthenticationOptions> options)
        {
            return authenticationBuilder.AddScheme<
                ApiKeyAuthenticationOptions, 
                ApiKeyAuthenticationHandler>
                (ApiKeyAuthenticationOptions.DefaultScheme, options);
        }
    }
}
