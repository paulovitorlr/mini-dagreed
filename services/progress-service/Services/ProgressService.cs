using progress_service.DTOs;
using progress_service.Models;
using progress_service.Repositories;

namespace progress_service.Services;

public class ProgressService : IProgressService
{
    private readonly IProgressRepository _repository;

    public ProgressService(IProgressRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProgressDTO?> EnrollAsync(int authId, EnrollDTO dto)
    {
        var existing = await _repository.GetByAuthIdAndCourseIdAsync(authId, dto.CourseId);
        if (existing != null) return null;

        var progress = new Progress
        {
            AuthId = authId,
            CourseId = dto.CourseId,
            TotalLessons = dto.TotalLessons,
            CompletedLessons = 0
        };

        await _repository.AddAsync(progress);
        await _repository.SaveChangesAsync();
        return MapToDTO(progress);
    }

    public async Task<ProgressDTO?> CompleteLessonAsync(int authId, CompleteLessonDTO dto)
    {
        var progress = await _repository.GetByAuthIdAndCourseIdAsync(authId, dto.CourseId);
        if (progress == null) return null;

        if (progress.CompletedLessons >= progress.TotalLessons) return MapToDTO(progress);

        progress.CompletedLessons += 1;
        progress.LastUpdated = DateTime.UtcNow;

        await _repository.UpdateAsync(progress);
        await _repository.SaveChangesAsync();
        return MapToDTO(progress);
    }

    public async Task<List<ProgressDTO>> GetAllByAuthIdAsync(int authId)
    {
        var progresses = await _repository.GetAllByAuthIdAsync(authId);
        return progresses.Select(MapToDTO).ToList();

    }

    private ProgressDTO MapToDTO(Progress progress)
    {
        return new ProgressDTO
        {
            Id = progress.Id,
            AuthId = progress.AuthId,
            CourseId = progress.CourseId,
            TotalLessons = progress.TotalLessons,
            CompletedLessons = progress.CompletedLessons,
            ProgressPercentage = progress.TotalLessons == 0 ? 0 :
                Math.Round((double)progress.CompletedLessons/ progress.TotalLessons * 100, 2),
            LastUpdated = progress.LastUpdated
        };
    }
}

