using System.Net;
using Microsoft.AspNetCore.Mvc;
using Safe_Notes_Api.Dto;
using Safe_Notes_Api.Services;

namespace Safe_Notes_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDto user)
        {
            var response =  await _authService.Register(user);
            if (response.Success) return Ok(response.Data);
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return Conflict("There exists user on given account");
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Some internal server error happend");
            }
            else return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login()
        {
            return Ok();
        }
        
    }
    
    
}

