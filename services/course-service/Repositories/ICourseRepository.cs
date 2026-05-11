using course_service.Models;

namespace course_service.Repositories;

public interface ICourseRepository
{
  Task<List<Course>>GetAllAsync();
  Task<Course?>GetByIdAsync(int id);
  Task UpdateAsync(Course course);
  Task AddAsync(Course course);
  Task DeleteAsync(Course course);
  Task SaveChangesAsync();
}

    
