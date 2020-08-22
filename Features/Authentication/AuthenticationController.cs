using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using HomeAutomation.IdentityService.Dto;
using HomeAutomation.IdentityService.Features.Authentication.ApiKeyAuthentication;
using HomeAutomation.IdentityService.Features.Authentication.ClientAuthentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeAutomation.IdentityService.Features.Authentication
{
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
       
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody]UserDto content)
        {
            try
            {
                var token = await _mediator.Send(new AuthenticateClientCommand(content.Email, content.Password));
                return Ok(new { token = _tokenHandler.WriteToken(token) });
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetTokenByApiKey()
        {
            try
            {
                var token = await _mediator.Send(new ApiKeyAuthenticationCommand(
                    User.Identity.Name,
                    User.Claims.ToList()[1].Value));
                return Ok(new { token = _tokenHandler.WriteToken(token) });
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }
    }
}
