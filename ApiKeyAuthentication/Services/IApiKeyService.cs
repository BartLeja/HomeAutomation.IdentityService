using HomeAutomation.IdentityService.ApiKeyAuthentication.Model;
using System.Threading.Tasks;

namespace HomeAutomation.IdentityService.ApiKeyAuthentication.Services
{
    public interface IApiKeyService
    {
        Task<ApiKey> Execute(string providedApiKey);
    }
}
