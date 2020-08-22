using HomeAutomation.IdentityService.ApiKeyAuthentication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAutomation.IdentityService.ApiKeyAuthentication.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IDictionary<string, ApiKey> _apiKeys;

        public ApiKeyService()
        {
            var existingApiKeys = new List<ApiKey>
        {
            new ApiKey(1, "LightingSystem", "C5BFF7F0-B4DF-475E-A331-F737424F013C", new DateTime(2020, 08, 21),
                new List<string>
                {
                    "LocalSystem",
                })
            };

            _apiKeys = existingApiKeys.ToDictionary(x => x.Key, x => x);
        }

        public Task<ApiKey> Execute(string providedApiKey)
        {
            _apiKeys.TryGetValue(providedApiKey, out var key);
            return Task.FromResult(key);
        }
    }
}
