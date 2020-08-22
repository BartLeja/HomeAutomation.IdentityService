using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeAutomation.IdentityService.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IDictionary _dictionaryCredentials;
        private static readonly SigningCredentials SigningCredentials = 
            new SigningCredentials(Startup.SecurityKey, SecurityAlgorithms.HmacSha256);

        public AuthenticationService()
        {
            //Add id Guid
            _dictionaryCredentials = new Dictionary<string, string>()
            {
                {"bleja", "test"},
                {"blejaService", "test"},
                {"bleja2", "test2"}
            };
        }
        public bool VerifyIfClientExist(string login, string password)
        {
            if (!_dictionaryCredentials.Contains(login))
            {
                throw new Exception("User not found");
            }

            if ((string)_dictionaryCredentials[login] != password)
            {
                throw new Exception("Incorrect password");
            }
            return true;
        }

        public async Task<JwtSecurityToken> CreateToken(string login)
        {
            var claim = new Claim(ClaimTypes.Name, login);
            var claims = new List<Claim> { claim };

           return new JwtSecurityToken(
                "SignalRAuthenticationSample",
                "SignalRAuthenticationSample",
                claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: SigningCredentials);
        }

        public async Task<JwtSecurityToken> CreateServiceToServiceToken(string serviceType, string homeAutomationLocalLightSystemId)
        {
            var serviceTypeClaim = new Claim(ClaimTypes.Name, serviceType);
            var serviceIdClaim = new Claim(ClaimTypes.PrimarySid, homeAutomationLocalLightSystemId);
            var claims = new List<Claim> { serviceTypeClaim, serviceIdClaim };

            return new JwtSecurityToken(
                 "SignalRAuthenticationSample",
                 "SignalRAuthenticationSample",
                 claims,
                 expires: DateTime.UtcNow.AddDays(30),
                 signingCredentials: SigningCredentials);
        }
    }
}
