using auth_service.Models;

namespace auth_service.Repositories;

    public interface IAuthRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task SaveChangesAsync();
       
    }

