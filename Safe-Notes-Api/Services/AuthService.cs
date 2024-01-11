using System.Net;
using System.Security.Cryptography;
using OtpNet;
using Safe_Notes_Api.Dto;
using Safe_Notes_Api.Models;

namespace Safe_Notes_Api.Services;

public class AuthService : IAuthService
{
    private readonly DatabaseContext _context;
    
    public AuthService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<RegisterResponseDto>> Register(RegisterRequestDto user)
    {
        if (_context.Users.FirstOrDefault(x => x.Email == user.Email) != null)
        {
            return new ServiceResponse<RegisterResponseDto>
            {
                Success = false,
                Message = "There exists user on given e-mail",
                StatusCode = HttpStatusCode.Conflict,
                Data = null,
            };
        }

        User newUser;

        using (var hmac = new HMACSHA512())
        {
            newUser = new User()
            {
                Email = user.Email,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password)),
                PasswordSalt = hmac.Key,
                Notes = new List<Note>(),
                TOTPSecret = KeyGeneration.GenerateRandomKey(20),
            };
        }

        RegisterResponseDto response = new RegisterResponseDto()
        {
            TOTPSecret = Base32Encoding.ToString(newUser.TOTPSecret),
        };

        try
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return new ServiceResponse<RegisterResponseDto>()
            {
                Success = true,
                Data = response,
                Message = "Successfully created account",
                StatusCode = HttpStatusCode.Created,
            };
        }
        catch(Exception exception)
        {
            return new ServiceResponse<RegisterResponseDto>()
            {
                Success = false,
                Data = null,
                Message = "Failed to create an account",
                StatusCode = HttpStatusCode.InternalServerError,

            };
        }
    }
}