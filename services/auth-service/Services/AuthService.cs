using auth_service.DTOs;
using auth_service.Models;
using auth_service.Repositories;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace auth_service.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repository;
    private readonly string __jwtSecret;

    public AuthService(IAuthRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        __jwtSecret = configuration["JwtSettings:Secret"]!;
    }

    public async Task<bool> RegisterAsync(RegisterDTO dto)
    {
        var existing = await _repository.GetByEmailAsync(dto.Email);
        if (existing != null) return false;

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _repository.AddAsync(user);
        await _repository.SaveChangesAsync();

        try
        {
            using var httpClient = new HttpClient();
            await httpClient.PostAsJsonAsync("https://localhost:7165/users/internal/create", new
            {
                AuthId = user.Id,
                Email = user.Email
            });
        }
        catch
        {
            // User Service indisponível, perfil será criado depois
        }
        return true;
    }
    public async Task<string?> LoginAsync(LoginDTO dto)
    {
        var user = await _repository.GetByEmailAsync(dto.Email);
        if (user == null) return null;

        var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!validPassword) return null;

        return GenerateToken(user);
    }

    public Task<bool> ValidateAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(__jwtSecret);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out _);

            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    private string GenerateToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(__jwtSecret);
        var tokenDescriptior = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptior);
        return tokenHandler.WriteToken(token);
    }
}

