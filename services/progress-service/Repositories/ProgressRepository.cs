using Microsoft.EntityFrameworkCore;
using progress_service.Data;
using progress_service.Models;

namespace progress_service.Repositories;

public class ProgressRepository : IProgressRepository
{
    private readonly AppDbContext _context;

    public ProgressRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Progress?> GetByAuthIdAndCourseIdAsync(int authId, int courseId)
    {
        return await _context.Progresses
            .FirstOrDefaultAsync(p => p.AuthId == authId && p.CourseId == courseId);
    }

    public async Task<List<Progress>> GetAllByAuthIdAsync(int authId)
    {
        return await _context.Progresses.Where(p => p.AuthId == authId).ToListAsync();
    }

    public async Task AddAsync(Progress progress)
    {
        await _context.Progresses.AddAsync(progress);
    }

    public async Task UpdateAsync(Progress progress) 
    {
        _context.Progresses.Update(progress);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

