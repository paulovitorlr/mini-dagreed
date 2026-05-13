namespace progress_service.Models;

public class Progress
{
    public int Id { get; set; }
    public int AuthId { get; set; }
    public int CourseId { get; set; }
    public int TotalLessons { get; set; }
    public int CompletedLessons { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

