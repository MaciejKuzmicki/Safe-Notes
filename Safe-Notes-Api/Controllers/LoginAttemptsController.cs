using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Safe_Notes_Api.Models;
using Safe_Notes_Api.Services;

namespace Safe_Notes_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginAttemptsController : ControllerBase
    {

        private readonly ILoginAttemptsService _loginAttemptsService;
        public LoginAttemptsController(ILoginAttemptsService loginAttemptsService)
        {
            _loginAttemptsService = loginAttemptsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetLoginAttempts([FromHeader] string UserId)
        {
            var response = await _loginAttemptsService.GetLoginAttempts(UserId);
            if (response.Success) return Ok(response.Data);
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(response.Message);
            }
            else return NoContent();
        }
    }
}