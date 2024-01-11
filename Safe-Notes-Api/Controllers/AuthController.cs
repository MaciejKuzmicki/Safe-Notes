using Microsoft.AspNetCore.Mvc;
using Safe_Notes_Api.Dto;

namespace Safe_Notes_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDto user)
        {
            
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login()
        {
            return Ok();
        }
        
    }
    
    
}

