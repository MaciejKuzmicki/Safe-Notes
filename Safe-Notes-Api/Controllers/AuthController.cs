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
                return Conflict(response.Message);
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, response.Message);
            }
            else return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto user)
        {
            var response = await _authService.Login(user);
            if (response.Success) return Ok(response.Data);
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(response.Message);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(response.Message);
            }
            else return NoContent();
        }
        
    }
    
    
}

