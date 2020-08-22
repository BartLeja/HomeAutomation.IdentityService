using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace HomeAutomation.IdentityService.Services
{
    public interface IAuthenticationService
    {
        bool VerifyIfClientExist(string login, string password);
        Task<JwtSecurityToken> CreateToken(string login);
        Task<JwtSecurityToken> CreateServiceToServiceToken(string serviceType, string serviceGuid);
    }
}
