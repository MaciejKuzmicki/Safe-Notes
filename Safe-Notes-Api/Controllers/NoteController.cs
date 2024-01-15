using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> AddNote()
        {
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult> GetNotes()
        {
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetNote([FromRoute] int id)
        {
            return NoContent();
        }
    }
}

