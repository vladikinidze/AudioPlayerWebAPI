namespace AudioPlayerWebAPI.Models
{
    public class Playlist
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Image { get; set; }
        public Guid ParentUserId { get; set; }
        public DateTime Created { get; set; }
        public ICollection<User> Users { get; set; } = null!;
        public ICollection<Track> Tracks { get; set; } = null!;
    }
}
