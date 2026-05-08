using auth_service.DTOs;

namespace auth_service.Services;

    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDTO dto);
        Task<string?> LoginAsync(LoginDTO dto);
        Task<bool> ValidateAsync(string token);
    }

