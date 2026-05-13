using progress_service.DTOs;

namespace progress_service.Services;
public interface IProgressService
{
    Task<ProgressDTO?> EnrollAsync(int authId, EnrollDTO dto);
    Task<ProgressDTO?> CompleteLessonAsync(int authId, CompleteLessonDTO dto);
    Task <List<ProgressDTO>> GetAllByAuthIdAsync(int authId);
}

