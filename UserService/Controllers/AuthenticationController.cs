using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Interfaces.Services;
using UserService.Models.Dto;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService )
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("logIn")]
        public async Task<IActionResult> LogIn(UserCredentials userCredentials)
        {
            return Ok(await _authenticationService.LoginAsync(userCredentials));
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationDto user)
        {
            return Ok(await _authenticationService.RegistrationAsync(user));
        }

        [HttpGet("refreshTokenVerification")]
        public async Task<IActionResult> RefreshTokenVerification([FromHeader] string refreshToken)
        {
            return Ok(await _authenticationService.RefreshTokenVerificationAsync(refreshToken));
        }

        [HttpGet("getTokens/{identifier}")]
        public async Task<IActionResult> TokenVerification(Guid identifier)
        {
            return Ok(await _authenticationService.GetTokensAsync(identifier));
        }
    }
}
