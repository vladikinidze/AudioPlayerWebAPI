namespace AudioPlayerWebAPI.Entities
{
    public class Playlist
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Image { get; set; }
        public bool Private { get; set; }
        public DateTime CreationDate { get; set; }

        public List<UserPlaylists> Users { get; set; }

        public List<PlaylistTracks> Tracks { get; set; }
    }
}
