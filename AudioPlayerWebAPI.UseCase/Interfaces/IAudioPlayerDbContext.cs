
using AudioPlayerWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Interfaces
{
    public interface IAudioPlayerDbContext
    {
        DbSet<Playlist> Playlists { get; set; }
        DbSet<Track> Tracks { get; set; }
        DbSet<UserPlaylists> UserPlaylists { get; set; }
        DbSet<PlaylistTracks> PlaylistTracks { get; set; }
        DbSet<Entities.User> Users { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        Task<int> SaveChangesAsync(CancellationToken  cancellationToken);
    }
}
