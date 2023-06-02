namespace AudioPlayerWebAPI.Entities
{
    public class UserPlaylists
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid PlaylistId { get; set; }
        public Playlist Playlist { get; set; } = null!;

        public bool IsOwner { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
