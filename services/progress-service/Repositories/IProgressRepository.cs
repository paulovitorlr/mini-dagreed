using progress_service.Models;

namespace progress_service.Repositories;

public interface IProgressRepository
{ 
    Task<Progress?> GetByAuthIdAndCourseIdAsync(int authId, int  courseId);
    Task<List<Progress>> GetAllByAuthIdAsync(int authId);
    Task AddAsync(Progress progress);
    Task UpdateAsync(Progress progress);
    Task SaveChangesAsync();
}

