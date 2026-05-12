namespace user_service.DTOs;

public class UserDTO
    {
        public int Id { get; set; }
        public int AuthId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Bio { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

