namespace AudioPlayerWebAPI.Entities
{
    public class UserPlaylists
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public bool IsOwner { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
