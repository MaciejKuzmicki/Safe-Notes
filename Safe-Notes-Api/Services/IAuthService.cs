using Safe_Notes_Api.Dto;
using Safe_Notes_Api.Models;

namespace Safe_Notes_Api.Services;

public interface IAuthService
{
    Task<ServiceResponse<RegisterResponseDto>> Register(RegisterRequestDto user);

    Task<ServiceResponse<LoginResponseDto>> Login(LoginRequestDto user, string ip);

}