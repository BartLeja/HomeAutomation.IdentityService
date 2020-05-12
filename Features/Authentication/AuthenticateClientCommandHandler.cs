using HomeAutomation.IdentityService.Services;
using MediatR;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace HomeAutomation.IdentityService.Features.Authentication
{
    public class AuthenticateClientCommandHandler : IRequestHandler<AuthenticateClientCommand, JwtSecurityToken>
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateClientCommandHandler(
            IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public Task<JwtSecurityToken> Handle(AuthenticateClientCommand request, CancellationToken cancellationToken)
        {
             _authenticationService.VerifyIfClientExist(request.Login, request.Password);

            return _authenticationService.CreateToken(request.Login, request.Password);
        }
    }
}
