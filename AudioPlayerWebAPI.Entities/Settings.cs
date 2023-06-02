namespace AudioPlayerWebAPI.Entities;

public class Settings
{
    public Guid Id { get; set; }
    public bool Explicit { get; set; }

    public User User { get; set; } = null!;
}