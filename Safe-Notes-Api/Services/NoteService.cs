
using System.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        if (currentUser.Notes == null) currentUser.Notes = new List<Note>();
        if (note.encrypted)
        {
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
        }
        else
        {
            newNote = new Note()
            {
                content = note.content,
                PasswordHash = new byte[0],
                PasswordSalt = new byte[0],
                isEncrypted = note.encrypted,
                isPublic = note.ispublic,
                title = note.title,
                UserId = currentUser.UserId,
                User = currentUser,
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
                Message = exception.Message,
                StatusCode = HttpStatusCode.InternalServerError,
                Data = null,
            };
        }
        
    }

    public async Task<ServiceResponse<NoteGetDto[]>> GetNotes()
    {
        var notes = await _context.Notes.Where(x => x.isPublic == true).ToListAsync();
        if (notes == null)
        {
            return new ServiceResponse<NoteGetDto[]>
            {
                Success = false,
                Message = "There is no public notes",
                StatusCode = HttpStatusCode.NotFound,
                Data = null,
            };
        }
        NoteGetDto[] notesToReturn = new NoteGetDto[notes.Count];
        for (int i = 0; i < notes.Count; i++)
        {
            notesToReturn[i] = new NoteGetDto()
            {
                content = notes[i].content,
                title = notes[i].title,
                encrypted = notes[i].isEncrypted,
            };
        }

        return new ServiceResponse<NoteGetDto[]>
        {
            Success = true,
            Message = "Success",
            StatusCode = HttpStatusCode.OK,
            Data = notesToReturn,
        };
    }

    public async Task<ServiceResponse<NoteGetDto[]>> GetMyNotes(string UserId)
    {
        var user = _context.Users.FirstOrDefault(x => x.UserId.ToString() == UserId);
    
        if (user == null)
        {
            return new ServiceResponse<NoteGetDto[]>
            {
                Success = false,
                Message = "User does not exist",
                Data = null,
                StatusCode = HttpStatusCode.NotFound,
            };
        }

        var notes = _context.Notes.Where(x => x.UserId.ToString() == UserId).ToList();

        if (!notes.Any())
        {
            return new ServiceResponse<NoteGetDto[]>
            {
                Success = false,
                Message = "User has no notes",
                Data = null,
                StatusCode = HttpStatusCode.NotFound,
            };
        }

        NoteGetDto[] notesToReturn = notes.Select(note => new NoteGetDto
        {
            content = note.content,
            title = note.title,
            encrypted = note.isEncrypted,
        }).ToArray();

        return new ServiceResponse<NoteGetDto[]>
        {
            Success = true,
            Message = "Success",
            StatusCode = HttpStatusCode.OK,
            Data = notesToReturn,
        };
    }



    
}