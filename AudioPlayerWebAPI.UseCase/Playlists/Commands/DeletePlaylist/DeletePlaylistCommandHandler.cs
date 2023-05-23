using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.DeletePlaylist
{
    public class DeletePlaylistCommandHandler : IRequestHandler<DeletePlaylistCommand, Unit>
    {
        private readonly IAudioPlayerDbContext _context;

        public DeletePlaylistCommandHandler(IAudioPlayerDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePlaylistCommand request, CancellationToken cancellationToken)
        {
            var userPlaylist = await _context.UserPlaylists
                .Include(up => up.Playlist)
                .FirstOrDefaultAsync(up => up.PlaylistId == request.Id
                                           && up.UserId == request.UserId, cancellationToken);

            if (userPlaylist is not { IsOwner: true })
            {
                throw new NotFoundException(nameof(Playlist), request.Id.ToString());
            }

            _context.Playlists.Remove(userPlaylist.Playlist);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
