namespace AudioPlayerWebAPI.Models
{
    public class Playlist
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Image { get; set; }
        public Guid ParentUserId { get; set; }
        public DateTime Created { get; set; }
        public List<User> Users { get; set; } = new();
        public List<Track> Tracks { get; set; } = new();
    }
}
