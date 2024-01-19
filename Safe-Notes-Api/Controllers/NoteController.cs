using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Safe_Notes_Api.Dto;
using Safe_Notes_Api.Services;

namespace Safe_Notes_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost]
        public async Task<ActionResult> AddNote([FromBody] NoteCreateDto note, [FromHeader] string UserId)
        {
            var response = await _noteService.AddNote(note, UserId);
            if (response.Success) return Ok();
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(response.Message);
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, response.Message);
            }
            else return NoContent();
        }

        [HttpGet("mynotes")]
        [Authorize]
        public async Task<ActionResult> GetMyNotes([FromHeader] string UserId)
        {
            var response = await _noteService.GetMyNotes(UserId);
            if (response.Success) return Ok(response.Data);
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(response.Message);
            }
            else return NoContent();        
        }

        [HttpGet]
        public async Task<ActionResult> GetNotes()
        {
            var response = await _noteService.GetNotes();
            if (response.Success) return Ok(response.Data);
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(response.Message);
            }
            else return NoContent();
        }

        [HttpPost("{noteId}")]
        public async Task<ActionResult> GetNote([FromRoute] string noteId, [FromHeader] string userId, [FromBody] NoteEncryptDto note )
        {
            var response = await _noteService.GetNote(userId, noteId, note);
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

