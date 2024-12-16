using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Tournament.Core.DTOs;

namespace Tournament.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public AuthController(IServiceManager serviceManager) 
        {
            _serviceManager = serviceManager;
        }

       [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(UserRegistrationDto userRegistrationDto)
        {
            var result = await _serviceManager.AuthService.RegisterUser(userRegistrationDto);
            return result.Succeeded ? StatusCode(StatusCodes.Status201Created) : BadRequest(result.Errors);

        }

        [HttpPost("login")]
        public async Task<ActionResult> Authenticate(UserAuthDto userAuthDto)
        {
            if( !await _serviceManager.AuthService.ValidateUserAsync(userAuthDto)) return Unauthorized();
            return Ok();
        }





    }
}
