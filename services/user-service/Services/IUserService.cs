using user_service.DTOs;

namespace user_service.Services;

public interface IUserService
{
    Task<UserDTO> CreateAsync(CreateUserDTO dto);
    Task<UserDTO?> GetByAuthIdAsync(int  authId);
    Task<UserDTO?> UpdateAsync(int authId, UpdateUserDTO dto);
}

