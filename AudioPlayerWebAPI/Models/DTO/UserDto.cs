namespace AudioPlayerWebAPI.Models.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
    }
}
