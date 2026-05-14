using recommendation_service.DTOs;

namespace recommendation_service.Services;

public interface IRecommendationService
{
    Task<List<RecommendationDTO>> GetRecommendationsAsync(int authIdm, string token);
}


