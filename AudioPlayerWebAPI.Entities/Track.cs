namespace AudioPlayerWebAPI.Entities
{
    public class Track
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Text { get; set; }
        public string Audio { get; set; } = null!;
        public bool Explicit { get; set; }
        public DateTime AddedDate { get; set; }

        public List<PlaylistTracks> Playlists { get; set; } = null!;
    }
}
