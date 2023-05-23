using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.Infrastructure.EntityTypeConfigurations;
using AudioPlayerWebAPI.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.Infrastructure
{
    public class AudioPlayerDbContext : DbContext, IAudioPlayerDbContext
    {
        public AudioPlayerDbContext(DbContextOptions<AudioPlayerDbContext> options) 
            : base(options) { }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<UserPlaylists> UserPlaylists { get; set; }
        public DbSet<PlaylistTracks> PlaylistTracks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlaylistConfiguration());
            modelBuilder.ApplyConfiguration(new TrackConfiguration());
            modelBuilder.ApplyConfiguration(new UserPlaylistsConfiguration());
            modelBuilder.ApplyConfiguration(new PlaylistTracksConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
