using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HomeAutomation.IdentityService.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeAutomation.IdentityService.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private static readonly SigningCredentials SigningCredentials = new SigningCredentials(Startup.SecurityKey, SecurityAlgorithms.HmacSha256);
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        
        private readonly IDictionary _dictionaryCredentials;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            //Add id Guid
            _dictionaryCredentials = new Dictionary<string, string>()
            {
                {"bleja", "test"},
                {"blejaService", "test"},
                {"bleja2", "test2"}
            };
            _logger = logger;
        }

        [HttpPost]
        public IActionResult GetToken([FromBody]UserDto content)
        {
            var email = content.Email;
            var password = content.Password;

            var claim = new Claim(ClaimTypes.Name, email);
            var claims = new List<Claim> {claim};

            if (!_dictionaryCredentials.Contains(email))
            {
               // _logger.LogInformation("User not found");
                return NotFound("User not found");
            }

            if ((string) _dictionaryCredentials[email] != password)
            {
                //_logger.LogInformation("Incorrect password");
                return Unauthorized("Incorrect password");
            }

            var token = new JwtSecurityToken(
                "SignalRAuthenticationSample",
                "SignalRAuthenticationSample",
                claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: SigningCredentials);
            return Ok(new { token = _tokenHandler.WriteToken(token) });
        }
    }
}
