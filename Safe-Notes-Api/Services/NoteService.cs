
using System.Net;
using System.Security.Cryptography;
using Safe_Notes_Api.Dto;
using Safe_Notes_Api.Models;

namespace Safe_Notes_Api.Services;

public class NoteService : INoteService
{
    private readonly DatabaseContext _context;
    private readonly JwtSettings _jwtSettings;
    public NoteService(DatabaseContext context, JwtSettings jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings;
    }
    public async Task<ServiceResponse<NoteCreateDto>> AddNote(NoteCreateDto note, string UserId)
    {
        User currentUser = _context.Users.FirstOrDefault(x => x.UserId.ToString() == UserId);
        if (currentUser == null)
        {
            return new ServiceResponse<NoteCreateDto>
            {
                Success = false,
                Message = "User does not exists",
                StatusCode = HttpStatusCode.NotFound,
                Data = null,
            };
        }
        Note newNote;
        using (var hmac = new HMACSHA512())
        {
            newNote = new Note()
            {
                content = note.content,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(note.password)),
                PasswordSalt = hmac.Key,
                isEncrypted = note.encrypted,
                isPublic = note.ispublic,
                title = note.title,
                UserId = currentUser.UserId,
            };
        }

        try
        {
            currentUser.Notes.Add(newNote);
            _context.Notes.Add(newNote);
            await _context.SaveChangesAsync();
            return new ServiceResponse<NoteCreateDto>()
            {
                Success = true,
                Data = note,
                Message = "Succesfully added note",
                StatusCode = HttpStatusCode.Created
            };
        }
        catch (Exception exception)
        {
            return new ServiceResponse<NoteCreateDto>
            {
                Success = false,
                Message = "Failed to add a note",
                StatusCode = HttpStatusCode.InternalServerError,
                Data = null,
            };
        }
        
    }

    
}