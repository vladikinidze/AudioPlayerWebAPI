using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.DeletePlaylistFromAdded;

public class DeletePlaylistFromAddedCommandHandler : IRequestHandler<DeletePlaylistFromAddedCommand, Unit>
{
    private readonly IAudioPlayerDbContext _context;

    public DeletePlaylistFromAddedCommandHandler(IAudioPlayerDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePlaylistFromAddedCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), request.UserId.ToString());
        }

        var userPlaylist = await _context.UserPlaylists.FirstOrDefaultAsync(x => x.PlaylistId == request.PlaylistId
            && x.UserId == request.UserId && !x.IsOwner, cancellationToken);

        if (userPlaylist == null)
        {
            throw new NotFoundException(nameof(Playlist), request.PlaylistId.ToString());
        }

        _context.UserPlaylists.Remove(userPlaylist);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}