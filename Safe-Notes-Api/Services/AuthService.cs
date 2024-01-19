using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OtpNet;
using Safe_Notes_Api.Dto;
using Safe_Notes_Api.Models;
using Safe_Notes_Api.Utils;

namespace Safe_Notes_Api.Services;

public class AuthService : IAuthService
{
    private readonly DatabaseContext _context;
    private readonly JwtSettings _jwtSettings;
    public AuthService(DatabaseContext context, JwtSettings jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings;
    }

    public async Task<ServiceResponse<LoginResponseDto>> Login(LoginRequestDto user, string ip)
    {
        User currentUser = _context.Users.FirstOrDefault(x => x.Email == user.Email);
        if (currentUser == null)
        {
            return new ServiceResponse<LoginResponseDto>()
            {
                Success = false,
                Message = "There is no account on given e-mail",
                StatusCode = HttpStatusCode.NotFound,
                Data = null,
            };
        }

        if (currentUser.LoginAttempts == null) currentUser.LoginAttempts = new List<LoginAttempt>();

        LoginAttempt loginAttempt = new LoginAttempt();
        loginAttempt.UserId = currentUser.UserId;
        loginAttempt.Time = DateTime.UtcNow;
        loginAttempt.IpAddress = ip;
        

        using (var hmac = new HMACSHA512(currentUser.PasswordSalt))
        {
            var givenHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password));
            for (int i = 0; i < givenHash.Length; i++)
            {
                if (givenHash[i] != currentUser.PasswordHash[i])
                {
                    loginAttempt.Success = false;
                    _context.LoginAttempts.Add(loginAttempt);
                    await _context.SaveChangesAsync();
                    return new ServiceResponse<LoginResponseDto>()
                    {
                        Success = false,
                        Message = "Wrong data",
                        StatusCode = HttpStatusCode.Unauthorized,
                        Data = null,
                    };
                }
            }
        }

        byte[] keyToDecrypt = AesEncryption.CreateAesKeyFromPassword(user.Password, currentUser.PasswordSalt);
        string decryptedTOTP = AesEncryption.Decrypt(Convert.ToBase64String(currentUser.TOTPSecret), Convert.ToBase64String(keyToDecrypt), currentUser.Iv);
        byte[] totpToConvert = Convert.FromBase64String(decryptedTOTP);

        var totp = new Totp(totpToConvert);
        if (!totp.VerifyTotp(user.TotpCode, out long timeStepMatched, new VerificationWindow(2, 2)))
        {
            loginAttempt.Success = false;
            _context.LoginAttempts.Add(loginAttempt);
            await _context.SaveChangesAsync();
            return new ServiceResponse<LoginResponseDto>()
            {
                Success = false,
                Message = "Wrong data",
                StatusCode = HttpStatusCode.Unauthorized,
                Data = null,
            };
        }

        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] 
            {
                new Claim(ClaimTypes.Email, currentUser.Email), 
                new Claim(ClaimTypes.NameIdentifier, currentUser.UserId.ToString()),
            }),
            Expires = DateTime.UtcNow.AddDays(1), 
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        LoginResponseDto response = new LoginResponseDto()
        {
            Token = tokenHandler.WriteToken(token),
        };
        loginAttempt.Success = true;
        _context.LoginAttempts.Add(loginAttempt);
        await _context.SaveChangesAsync();

        return new ServiceResponse<LoginResponseDto>()
        {
            Success = true,
            Message = "Succesfully logged in",
            StatusCode = HttpStatusCode.OK,
            Data = response,
        };
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
        string iv;
        using (Aes aes = Aes.Create())
        {
            aes.GenerateIV();
            iv = Convert.ToBase64String(aes.IV);
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
                LoginAttempts = new List<LoginAttempt>(),
                TOTPSecret = KeyGeneration.GenerateRandomKey(20),
                Iv = iv,
            };
        }

        RegisterResponseDto response = new RegisterResponseDto()
        {
            TOTPSecret = Base32Encoding.ToString(newUser.TOTPSecret),
        };
        

        byte[] key = AesEncryption.CreateAesKeyFromPassword(user.Password, newUser.PasswordSalt);
        string totpSecretBase64 = Convert.ToBase64String(newUser.TOTPSecret);
        newUser.TOTPSecret = Convert.FromBase64String(AesEncryption.Encrypt(totpSecretBase64, Convert.ToBase64String(key), newUser.Iv));

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