namespace user_service.Messaging;

public class UserCreatedEvent
{
    public int AuthId { get; set; }
    public string Email { get; set; } = string.Empty;
}

