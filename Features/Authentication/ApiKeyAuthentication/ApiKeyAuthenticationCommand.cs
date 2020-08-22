using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace HomeAutomation.IdentityService.Features.Authentication.ApiKeyAuthentication
{
    public class ApiKeyAuthenticationCommand : IRequest<JwtSecurityToken>
    {
        public string Login { get; set; }
        public string HomeAutomationLocalLightSystemId { get; set; }

        public ApiKeyAuthenticationCommand(string login, string homeAutomationLocalLightSystemId) {
            Login = login;
            HomeAutomationLocalLightSystemId = homeAutomationLocalLightSystemId;
        } 
    }
}
