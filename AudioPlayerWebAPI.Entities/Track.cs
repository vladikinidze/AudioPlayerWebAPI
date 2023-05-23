namespace AudioPlayerWebAPI.Entities
{
    public class Track
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Text { get; set; }
        public string Audio { get; set; }
        public bool Explicit { get; set; }
        public DateTime AddedDate { get; set; }

        public List<PlaylistTracks> Playlists { get; set; }
    }
}
