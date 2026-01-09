using ServiceManagementApi.Data;
using ServiceManagementApi.DTOs.Auth;
using ServiceManagementApi.Helpers;
using ServiceManagementApi.Models;
using ServiceManagementApi.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ServiceManagementApi.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtTokenHelper _jwtHelper;

    public AuthService(ApplicationDbContext context, JwtTokenHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    public string Register(RegisterDto dto)
    {
        if (_context.Users.Any(u => u.Email == dto.Email))
            throw new Exception("Email already registered");

        var allowedRoles = new[] { "Customer", "Admin", "Technician", "ServiceManager" };

        if (!allowedRoles.Contains(dto.Role))
            throw new Exception("Invalid role selected");

        var user = new AppUser
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = HashPassword(dto.Password),
            Role = dto.Role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        if (dto.Role == "Technician")
        {
            var technician = new Technician
            {
                UserId = user.UserId,
                Name = user.FullName,
                IsAvailable = true,
                IsActive = true
            };

            _context.Technicians.Add(technician);
            _context.SaveChanges();
        }

        return "User registered successfully";
    }

    public string Login(LoginDto dto)
    {
        var user = _context.Users.FirstOrDefault(
            u => u.Email == dto.Email && u.IsActive);

        if (user == null || user.PasswordHash != HashPassword(dto.Password))
            throw new Exception("Invalid credentials");

        return _jwtHelper.GenerateToken(user);
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        return Convert.ToBase64String(
            sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
}
