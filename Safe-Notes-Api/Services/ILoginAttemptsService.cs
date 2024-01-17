using Safe_Notes_Api.Dto;
using Safe_Notes_Api.Models;

namespace Safe_Notes_Api.Services;

public interface ILoginAttemptsService
{
    Task<ServiceResponse<LoginAttemptDto []>> GetLoginAttempts(string userId);

}