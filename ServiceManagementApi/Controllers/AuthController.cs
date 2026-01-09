using Microsoft.AspNetCore.Mvc;
using ServiceManagementApi.DTOs.Auth;
using ServiceManagementApi.Services.Interfaces;

namespace ServiceManagementApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        var result = _authService.Register(dto);
        return Ok(new { message = result });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var token = _authService.Login(dto);
        return Ok(new { token });
    }
}
