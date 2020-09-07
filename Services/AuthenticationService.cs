using HomeAutomation.IdentityService.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeAutomation.IdentityService.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly Dictionary<string, User> _dictionaryCredentials;
        private static readonly SigningCredentials SigningCredentials = 
            new SigningCredentials(Startup.SecurityKey, SecurityAlgorithms.HmacSha256);

        public AuthenticationService()
        {
            //Add id Guid
            _dictionaryCredentials = new Dictionary<string, User>()
            {
                {
                    "bleja", 
                    new User(
                        "bleja",
                        "test",
                        Guid.Parse("bdcd95ec-2dc6-4a7b-90ca-f132a7784b0f"))
                },
            };
        }
        public bool VerifyIfClientExist(string login, string password)
        {
            if (!_dictionaryCredentials.ContainsKey(login))
            {
                throw new Exception("User not found");
            }

            if (_dictionaryCredentials[login].Password != password)
            {
                throw new Exception("Incorrect password");
            }
            return true;
        }

        public async Task<JwtSecurityToken> CreateToken(string login)
        {
          //  var id = _dictionaryCredentials.First(u => u.Key == login);
            var claim = new Claim(ClaimTypes.Name, login);
            var homeAutomationId = new Claim("homeAutomationId", 
                _dictionaryCredentials.First(u => u.Key == login).Value.HomeAutomationGuid.ToString());
            var claims = new List<Claim> { claim, homeAutomationId };

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
            var id = new Claim("Id", homeAutomationLocalLightSystemId);
            var claims = new List<Claim> { serviceTypeClaim, id };

            return new JwtSecurityToken(
                 "SignalRAuthenticationSample",
                 "SignalRAuthenticationSample",
                 claims,
                 expires: DateTime.UtcNow.AddDays(30),
                 signingCredentials: SigningCredentials);
        }
    }
}
