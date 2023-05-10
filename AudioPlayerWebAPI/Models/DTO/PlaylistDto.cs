namespace AudioPlayerWebAPI.Models.DTO
{
    public class PlaylistDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Image { get; set; }
        public Guid ParentUserId { get; set; }
        public UserDto User { get; set; }
        public DateTime Created { get; set; }
    }
}
