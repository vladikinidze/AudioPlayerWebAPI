namespace AudioPlayerWebAPI.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Image { get; set; }
        public string Password { get; set; }

        public List<UserPlaylists> Playlists { get; set; }

        public Guid RefreshTokenId { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
