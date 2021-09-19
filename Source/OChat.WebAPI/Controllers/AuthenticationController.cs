using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OChat.WebAPI.Models;
using OChat.Core.Services;

namespace OChat.WebAPI.Controllers
{
    using static AuthenticationRoutes;
    using static AuthenticationResponses;

    [Route(AUTHENTICATION)]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(AuthenticationService authenticationService) 
            => _authenticationService = authenticationService;

        [AllowAnonymous]
        [HttpPost(TOKEN, Name = nameof(GetToken))]
        public async Task<IActionResult> GetToken([FromBody] Login request)
        {
            var authResult = await _authenticationService.Authenticate(request.Username, request.Password);

            if (!authResult.IsAuthenticated)
                return Unauthorized(SIGN_IN_FAILED);

            return Ok(authResult.Token);
        }

        [AllowAnonymous]
        [HttpPost(REGISTER, Name = nameof(Register))]
        public async Task<IActionResult> Register(
            [FromBody] Login request)
        {
            await _authenticationService.RegisterUser(request.Username, request.Email, request.Password);

            return Ok();
        }
    }
}
