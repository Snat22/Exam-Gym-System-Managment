using Domain.DTOs.AuthDto;
using Domain.DTOs.UserDto;
using Domain.Responses;

namespace Infrastructure.Services.AuthService;

public interface IAuthService
{
    public Task<Response<string>> Login(LoginDto loginDto);

    public Task<Response<string>> RegisterUser(RegisterDto registerDto);
}
