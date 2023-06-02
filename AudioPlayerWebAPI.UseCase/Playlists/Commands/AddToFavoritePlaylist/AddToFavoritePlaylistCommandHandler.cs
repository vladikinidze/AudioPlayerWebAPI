using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.AddToFavoritePlaylist;

public class AddToFavoritePlaylistCommandHandler : IRequestHandler<AddToFavoritePlaylistCommand, Unit>
{
    private readonly IAudioPlayerDbContext _context;

    public AddToFavoritePlaylistCommandHandler(IAudioPlayerDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(AddToFavoritePlaylistCommand request, CancellationToken cancellationToken)
    {
        var track = await _context.Tracks
            .FirstOrDefaultAsync(x => x.Id == request.TrackId, cancellationToken);
        if (track == null)
        {
            throw new NotFoundException(nameof(Tracks), request.TrackId.ToString());
        }

        var userPlaylist = await _context.UserPlaylists
            .Include(x => x.Playlist)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId
                                      && x.Playlist.Title == "Favorite", cancellationToken);
        if (userPlaylist == null)
        {
            throw new NotFoundException(nameof(User), request.UserId.ToString());
        }

        var playlistTrack = new PlaylistTracks
        {
            Playlist = userPlaylist.Playlist,
            TrackId = track.Id,
            Track = track,
            AddedTime = DateTime.Now,
            IsParent = false,
            PlaylistId = userPlaylist.PlaylistId
        };

        await _context.PlaylistTracks.AddAsync(playlistTrack, cancellationToken);
        return Unit.Value;
    }
}