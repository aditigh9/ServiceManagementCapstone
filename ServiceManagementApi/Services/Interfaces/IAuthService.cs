using ServiceManagementApi.DTOs.Auth;

namespace ServiceManagementApi.Services.Interfaces;

public interface IAuthService
{
    string Register(RegisterDto dto);
    string Login(LoginDto dto);
}
