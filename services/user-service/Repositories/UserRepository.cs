using Microsoft.EntityFrameworkCore;
using user_service.Data;
using user_service.Models;

namespace user_service.Repositories;

public class UserRepository : IUserRepository
{
        private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) 
        {
            _context = context;
        }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> GetByAuthIdAsync(int authId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.AuthId == authId);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task UpdateAsync(User user) 
    {
        _context.Users.Update(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

