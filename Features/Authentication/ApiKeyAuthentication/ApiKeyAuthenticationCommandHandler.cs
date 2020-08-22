using HomeAutomation.IdentityService.Services;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace HomeAutomation.IdentityService.Features.Authentication.ApiKeyAuthentication
{
    public class ApiKeyAuthenticationCommandHandler : IRequestHandler<ApiKeyAuthenticationCommand, JwtSecurityToken>
    {
        private readonly IAuthenticationService _authenticationService;

        public ApiKeyAuthenticationCommandHandler(IAuthenticationService authenticationService) => 
            _authenticationService = authenticationService;


        public Task<JwtSecurityToken> Handle(ApiKeyAuthenticationCommand request, CancellationToken cancellationToken)
        {
            return _authenticationService.CreateServiceToServiceToken(request.Login,request.HomeAutomationLocalLightSystemId);
        }
    }
}
