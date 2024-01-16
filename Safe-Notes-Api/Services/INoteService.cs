using Safe_Notes_Api.Dto;
using Safe_Notes_Api.Models;

namespace Safe_Notes_Api.Services;

public interface INoteService
{
    Task<ServiceResponse<NoteCreateDto>> AddNote(NoteCreateDto note, string UserId);

    Task<ServiceResponse<NoteGetDto[]>> GetNotes();
    
    Task<ServiceResponse<NoteGetDto[]>> GetMyNotes(string UserId);


}