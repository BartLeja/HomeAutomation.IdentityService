using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace HomeAutomation.IdentityService.Features.Authentication.ClientAuthentication
{
    public class AuthenticateClientCommand : IRequest<JwtSecurityToken>
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public AuthenticateClientCommand(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
