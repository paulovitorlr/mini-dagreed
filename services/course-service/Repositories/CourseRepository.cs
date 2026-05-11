using Microsoft.EntityFrameworkCore;
using course_service.Data;
using course_service.Models;

namespace course_service.Repositories;

  public class CourseRepository : ICourseRepository
{
        private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context) 
    {
        _context = context;
    }

    public async Task<List<Course>> GetAllAsync()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
    }
    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
    }
    
    public async Task DeleteAsync(Course course)
    {
        _context.Courses.Remove(course);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
  }

