using course_service.DTOs;
using course_service.Models;
using course_service.Repositories;

namespace course_service.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;

    public CourseService(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CourseDTO>> GetAllAsync()
    {
        var courses = await _repository.GetAllAsync();
        return courses.Select(MapToDTO).ToList();
    }

    public async Task<CourseDTO?> GetByIdAsync(int id) 
    {
     var course = await _repository.GetByIdAsync(id);
        if (course == null) return null;
        return MapToDTO(course);
    }

    public async Task<CourseDTO> CreateAsync(CreateCourseDTO dto)
    {
        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            ImageURL = dto.ImageUrl,
            Category = dto.Category
        };
        await _repository.AddAsync(course);
        await _repository.SaveChangesAsync();
        return MapToDTO(course);
    }

    public async Task<CourseDTO?> UpdateAsync(int id, UpdateCourseDTO dto)
    {
        var course = await _repository.GetByIdAsync(id);
        if (course == null) return null;

        course.Title = dto.Title;
        course.Description = dto.Description;
        course.Price = dto.Price;
        course.ImageURL = dto.ImageUrl;
        course.Category = dto.Category;

        await _repository.UpdateAsync(course);
        await _repository.SaveChangesAsync();
        return MapToDTO(course);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _repository.GetByIdAsync(id);
        if (course == null ) return false;

        await _repository.DeleteAsync(course);
        await _repository.SaveChangesAsync();
        return true;
    }

    private CourseDTO MapToDTO(Course course)
    {
        return new CourseDTO
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Price = course.Price,
            ImageUrl = course.ImageURL,
            Category = course.Category,
            CreatedAt = course.CreatedAt,
        };
    }
    
}

