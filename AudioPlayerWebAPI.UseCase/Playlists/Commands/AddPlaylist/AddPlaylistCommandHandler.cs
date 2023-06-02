using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.AddPlaylist;

public class AddPlaylistCommandHandler : IRequestHandler<AddPlaylistCommand, Unit>
{
    private readonly IAudioPlayerDbContext _context;

    public AddPlaylistCommandHandler(IAudioPlayerDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddPlaylistCommand request, CancellationToken cancellationToken)
    {
        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(x => x.Id == request.PlaylistId, cancellationToken);
        if (playlist == null)
        {
            throw new NotFoundException(nameof(Playlist), request.PlaylistId.ToString());
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), request.UserId.ToString());
        }

        var userPlaylist = new UserPlaylists
        {
            PlaylistId = playlist.Id,
            User = user,
            UserId = user.Id,
            Playlist = playlist,
            AddedDate = DateTime.Now,
            IsOwner = false
        };
        await _context.UserPlaylists.AddAsync(userPlaylist, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}