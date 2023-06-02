namespace AudioPlayerWebAPI.Entities
{
    public class PlaylistTracks
    {
        public Guid PlaylistId { get; set; }
        public Playlist Playlist { get; set; } = null!;

        public Guid TrackId { get; set; }
        public Track Track { get; set; } = null!;

        public bool IsParent { get; set; }
        public DateTime AddedTime { get; set; }
    }
}
