using System.Globalization;

namespace recommendation_service.DTOs;

public class RecommendationDTO
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description {  get; set; } = string.Empty;
    public decimal Price { get; set; }
}

