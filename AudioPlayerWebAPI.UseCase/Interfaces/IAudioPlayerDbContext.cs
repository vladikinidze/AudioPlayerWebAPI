
using AudioPlayerWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Interfaces;

public interface IAudioPlayerDbContext
{
    DbSet<Playlist> Playlists { get; set; }
    DbSet<Track> Tracks { get; set; }
    DbSet<UserPlaylists> UserPlaylists { get; set; }
    DbSet<PlaylistTracks> PlaylistTracks { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }
    DbSet<Settings> Settings { get; set; }
    DbSet<Error> Errors { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}