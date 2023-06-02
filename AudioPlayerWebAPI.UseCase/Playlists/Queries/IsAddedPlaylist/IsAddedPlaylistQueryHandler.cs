using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.IsAddedPlaylist;

public class IsAddedPlaylistQueryHandler : IRequestHandler<IsAddedPlaylistQuery, bool>
{
    private readonly IAudioPlayerDbContext _context;

    public IsAddedPlaylistQueryHandler(IAudioPlayerDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(IsAddedPlaylistQuery request, CancellationToken cancellationToken)
    {
        var userPlaylist = await _context.UserPlaylists
            .FirstOrDefaultAsync(x => x.PlaylistId == request.PlaylistId
                                      && x.UserId == request.UserId && !x.IsOwner, cancellationToken);
        return userPlaylist != null;
    }
}