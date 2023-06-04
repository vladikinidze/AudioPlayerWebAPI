using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.IsFavoriteTrack;

public class IsFavoriteTrackQueryHandler : IRequestHandler<IsFavoriteTrackQuery, bool>
{
    private readonly IAudioPlayerDbContext _context;

    public IsFavoriteTrackQueryHandler(IAudioPlayerDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(IsFavoriteTrackQuery request, CancellationToken cancellationToken)
    {
        var playlistUser = await _context.UserPlaylists
            .Include(x => x.Playlist)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId
                                      && x.Playlist.Title == "Favorite", cancellationToken);

        if (playlistUser != null)
        {
            var playlistTrack = await _context.PlaylistTracks
                .FirstOrDefaultAsync(x => x.PlaylistId == playlistUser.PlaylistId 
                                          && x.TrackId == request.TrackId, cancellationToken);
            return playlistTrack != null;
        }

        throw new NotFoundException(nameof(Track), request.TrackId.ToString());
    }
}