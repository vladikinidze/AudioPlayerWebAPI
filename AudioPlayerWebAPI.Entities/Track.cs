namespace AudioPlayerWebAPI.Entities
{
    public class Track
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Audio { get; set; } = null!;
        public double Duration { get; set; }
        public bool Explicit { get; set; }
        public DateTime AddedDate { get; set; }

        public List<PlaylistTracks> Playlists { get; set; } = null!;
    }
}
