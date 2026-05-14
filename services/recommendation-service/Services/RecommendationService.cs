using System.Net.Http.Headers;
using System.Text.Json;
using recommendation_service.DTOs;

namespace recommendation_service.Services;

public class RecommendationService : IRecommendationService
{
    private readonly HttpClient _httpClient;
    private const string ProgressServiceUrl = "https://localhost:7166";
    private const string CourseServiceUrl = "https://localhost:7164";

    public RecommendationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<RecommendationDTO>> GetRecommendationsAsync(int authId, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        //busca progresso do usuario
        var progressResponse = await _httpClient.GetAsync($"{ProgressServiceUrl}/progress/my-progress");
        var progressJson = await progressResponse.Content.ReadAsStringAsync();
        var progresses = JsonSerializer.Deserialize<List<ExternalProgressDTO>>(progressJson, options) ?? new();

        //busca todos os cursos
        var coursesResponse = await _httpClient.GetAsync($"{CourseServiceUrl}/courses");
        var coursesJson = await coursesResponse.Content.ReadAsStringAsync();
        var courses = JsonSerializer.Deserialize<List<ExternalCourseDTO>>(coursesJson, options) ?? new();

        //pega categorias que o usuario ja esta matriculado
        var enrolledCourseIds = progresses.Select(p => p.CourseId).ToHashSet();
        var enrolledCategories = courses
            .Where(c => enrolledCourseIds.Contains(c.Id))
            .Select(c => c.Category)
            .ToHashSet();

        //recomenda cursos da mesma categoria que ainda nao começou
        var recommendations = courses
            .Where(c => !enrolledCourseIds.Contains(c.Id) && enrolledCategories.Contains(c.Category))
            .Select(c => new RecommendationDTO
            {
                CourseId = c.Id,
                Title = c.Title,
                Category = c.Category,
                Description = c.Description,
                Price = c.Price,
            }).ToList();

        return recommendations;
    }
}

