using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteFromFavoritePlaylist;

public class DeleteFromFavoritePlaylistCommandHandler : IRequestHandler<DeleteFromFavoritePlaylistCommand, Unit>
{
    private readonly IAudioPlayerDbContext _context;

    public DeleteFromFavoritePlaylistCommandHandler(IAudioPlayerDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(DeleteFromFavoritePlaylistCommand request, CancellationToken cancellationToken)
    {
        var userPlaylist = await _context.UserPlaylists
            .Include(x => x.Playlist)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId
                                      && x.Playlist.Title == "Favorite", cancellationToken);
        if (userPlaylist == null)
        {
            throw new NotFoundException(nameof(User), request.UserId.ToString());
        }

        var playlistTrack = await _context.PlaylistTracks
            .FirstOrDefaultAsync(x => x.PlaylistId == userPlaylist.PlaylistId
                                      && x.TrackId == request.TrackId, cancellationToken);
        if (playlistTrack == null)
        {
            throw new NotFoundException(nameof(Track), request.TrackId.ToString());

        }
        _context.PlaylistTracks.Remove(playlistTrack);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}