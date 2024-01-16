
using System.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Safe_Notes_Api.Dto;
using Safe_Notes_Api.Models;
using Safe_Notes_Api.Utils;

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
            string key, iv;
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();
                key = Convert.ToBase64String(aes.Key);
                iv = Convert.ToBase64String(aes.IV);
            }
            AesEncryption aesEncryption = new AesEncryption(key, iv);
            
            using (var hmac = new HMACSHA512())
            {
                newNote = new Note()
                {
                    content = aesEncryption.Encrypt(note.content),
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(note.password)),
                    PasswordSalt = hmac.Key,
                    isEncrypted = note.encrypted,
                    isPublic = note.ispublic,
                    title = note.title,
                    UserId = currentUser.UserId,
                    key = key,
                    iv=iv,
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
                key = "",
                iv="",
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
                noteId = notes[i].NoteId.ToString(),
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

        NoteGetDto[] notesToReturn = new NoteGetDto[notes.Count];
        for (int i = 0; i < notes.Count; i++)
        {
            string content;
            if (notes[i].isEncrypted) content = "This content is encrypted";
            else content = notes[i].content;
            notesToReturn[i] = new NoteGetDto()
            {
                content = content,
                title = notes[i].title,
                encrypted = notes[i].isEncrypted,
                noteId = notes[i].NoteId.ToString(),
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

    public async Task<ServiceResponse<NoteGetDto>> GetNote(string UserId, string NoteId, NoteEncryptDto noteEncryptDto)
    {
        Note note = await _context.Notes.Where(x => x.UserId.ToString() == UserId && x.NoteId.ToString() == NoteId).FirstOrDefaultAsync();
        if (note == null)
        {
            return new ServiceResponse<NoteGetDto>
            {
                Success = false,
                Message = "Note does not exist",
                Data = null,
                StatusCode = HttpStatusCode.NotFound,
            };
        }
        
        using (var hmac = new HMACSHA512(note.PasswordSalt))
        {
            var givenHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(noteEncryptDto.password));
            for (int i = 0; i < givenHash.Length; i++)
            {
                if (givenHash[i] != note.PasswordHash[i])
                {
                    return new ServiceResponse<NoteGetDto>
                    {
                        Success = false,
                        Message = "Wrong password",
                        Data = null,
                        StatusCode = HttpStatusCode.Unauthorized,
                    };
                }
            }
        }

        AesEncryption aes = new AesEncryption(note.key, note.iv);

        NoteGetDto noteToReturn = new NoteGetDto()
        {
            content = aes.Decrypt(note.content),
            title = note.title,
            encrypted = false,
            noteId = note.NoteId.ToString(),
        };

        return new ServiceResponse<NoteGetDto>
        {
            Success = true,
            Message = "Successful",
            Data = noteToReturn,
            StatusCode = HttpStatusCode.OK,
        };



    }




    
}