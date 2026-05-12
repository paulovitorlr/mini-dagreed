using user_service.DTOs;
using user_service.Models;
using user_service.Repositories;

namespace user_service.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserDTO> CreateAsync(CreateUserDTO dto)
    {
        var user = new User
        {
            AuthId = dto.AuthId,
            Email = dto.Email,
        };
        await _repository.AddAsync(user);
        await _repository.SaveChangesAsync();
        return MapToDTO(user);
    }

    public async Task<UserDTO?> GetByAuthIdAsync(int authId) 
    {
        var user = await _repository.GetByAuthIdAsync(authId);
        if (user == null) return null;
        return MapToDTO(user);
    }

    public async Task<UserDTO?> UpdateAsync(int authId, UpdateUserDTO dto)
    {
        var user = await _repository.GetByAuthIdAsync(authId);
        if (user == null) return null;

        user.Name = dto.Name;
        user.Age = dto.Age;
        user.Bio = dto.Bio;

        await _repository.UpdateAsync(user);
        await _repository.SaveChangesAsync();
        return MapToDTO(user);
    }

    private UserDTO MapToDTO(User user) 
    {
        return new UserDTO
        {
            Id = user.Id,
            AuthId = user.AuthId,
            Name = user.Name,
            Email = user.Email,
            Age = user.Age,
            Bio = user.Bio,
            CreatedAt = user.CreatedAt,
        };
    }
}

