using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.UpdatePlaylist
{
    public class UpdatePlaylistCommandHandler : IRequestHandler<UpdatePlaylistCommand, Unit>
    {
        private readonly IAudioPlayerDbContext _context;

        public UpdatePlaylistCommandHandler(IAudioPlayerDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePlaylistCommand request, CancellationToken cancellationToken)
        {
            var userPlaylist = await _context.UserPlaylists
                .Include(up => up.Playlist)
                .FirstOrDefaultAsync(up => up.PlaylistId == request.Id 
                                           && up.UserId == request.UserId, cancellationToken);

            if (userPlaylist is not { IsOwner: true })
            {
                throw new NotFoundException(nameof(Playlist), request.Id.ToString());
            }

            userPlaylist.Playlist.Title = request.Title;
            userPlaylist.Playlist.Image = request.Image;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
