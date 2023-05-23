namespace AudioPlayerWebAPI.Entities
{
    public class PlaylistTracks
    {
        public Guid PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public Guid TrackId { get; set; }
        public Track Track { get; set; }

        public bool IsParent { get; set; }
        public DateTime AddedTime { get; set; }
    }
}
