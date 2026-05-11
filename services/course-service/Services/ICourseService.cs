using course_service.DTOs;

namespace course_service.Services;

public interface ICourseService
{
    Task<List<CourseDTO>> GetAllAsync();
    Task<CourseDTO?> GetByIdAsync(int id);
    Task<CourseDTO?> CreateAsync(CreateCourseDTO dto);
    Task<CourseDTO?> UpdateAsync(int id, UpdateCourseDTO dto);
    Task<bool> DeleteAsync(int id);
}

