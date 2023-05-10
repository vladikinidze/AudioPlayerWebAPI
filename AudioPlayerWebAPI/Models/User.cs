namespace AudioPlayerWebAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<Playlist> Playlists { get; set; } = new();
        public Guid RefreshTokenId { get; set; }
        public UserRefreshToken RefreshToken { get; set; }
    }
}
