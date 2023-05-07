namespace AudioPlayerWebAPI.Models
{
    public class Playlist
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Image { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime Created { get; set; }
        public ICollection<Track> Tracks { get; set; } = null!;
    }
}
