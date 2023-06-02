using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.Infrastructure.EntityTypeConfigurations;
using AudioPlayerWebAPI.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.Infrastructure;

public class AudioPlayerDbContext : DbContext, IAudioPlayerDbContext
{
    public AudioPlayerDbContext(DbContextOptions<AudioPlayerDbContext> options) 
        : base(options) { }

    public DbSet<Playlist> Playlists { get; set; } = null!;
    public DbSet<Track> Tracks { get; set; } = null!;
    public DbSet<UserPlaylists> UserPlaylists { get; set; } = null!;
    public DbSet<PlaylistTracks> PlaylistTracks { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<Settings> Settings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.RefreshToken)
            .WithOne(r => r.User)
            .HasForeignKey<RefreshToken>(r => r.Id);
        modelBuilder.Entity<User>()
            .HasOne(u => u.Settings)
            .WithOne(r => r.User)
            .HasForeignKey<Settings>(r => r.Id);
        modelBuilder.ApplyConfiguration(new PlaylistConfiguration());
        modelBuilder.ApplyConfiguration(new TrackConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserPlaylistsConfiguration());
        modelBuilder.ApplyConfiguration(new PlaylistTracksConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}