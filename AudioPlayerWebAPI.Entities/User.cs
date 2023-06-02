namespace AudioPlayerWebAPI.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Image { get; set; }
    public string Password { get; set; } = null!;

    public List<UserPlaylists> Playlists { get; set; } = null!;
        
    public RefreshToken RefreshToken { get; set; } = null!;
    public Settings Settings { get; set; } = null!;
}