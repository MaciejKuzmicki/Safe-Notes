using System.Net;
using Safe_Notes_Api.Dto;
using Safe_Notes_Api.Models;

namespace Safe_Notes_Api.Services;

public class LoginAttemptsService : ILoginAttemptsService
{
    private readonly DatabaseContext _context;

    public LoginAttemptsService(DatabaseContext context)
    {
        _context = context;
    }
    public async Task<ServiceResponse<LoginAttemptDto[]>> GetLoginAttempts(string userId)
    {
        User currentUser = _context.Users.FirstOrDefault(x => x.UserId.ToString() == userId);
        if (currentUser == null)
        {
            return new ServiceResponse<LoginAttemptDto[]>
            {
                Success = false,
                Message = "User does not exists",
                StatusCode = HttpStatusCode.NotFound,
                Data = null,
            };
        }
        
        var loginattempts = _context.LoginAttempts.Where(x => x.UserId.ToString() == userId).ToList().OrderByDescending(l=>l.Time).ToList();
        LoginAttemptDto[] loginAttemptToReturn = new LoginAttemptDto[loginattempts.Count];
        for (int i = 0; i < loginattempts.Count; i++)
        {
            loginAttemptToReturn[i] = new LoginAttemptDto()
            {
                IpAddress = loginattempts[i].IpAddress,
                Success = loginattempts[i].Success,
                Time = loginattempts[i].Time,
                ClientName = loginattempts[i].ClientName,
            };
        }
        
        return new ServiceResponse<LoginAttemptDto[]>
        {
            Success = true,
            Message = "Success",
            StatusCode = HttpStatusCode.OK,
            Data = loginAttemptToReturn,
        };


    }
}