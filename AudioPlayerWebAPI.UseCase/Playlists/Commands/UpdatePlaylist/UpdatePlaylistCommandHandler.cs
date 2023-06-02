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
            userPlaylist.Playlist.Private = request.Private;

            if (request.Image != null)
            {
                if (userPlaylist.Playlist.Image != null && userPlaylist.Playlist.Image != "548864f8-319e-40ac-9f9b-a31f65ccb902.jpg")
                {
                    var image = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files\\Image", userPlaylist.Playlist.Image);
                    if (File.Exists(image))
                    {
                        File.Delete(image);
                    }
                }
                userPlaylist.Playlist.Image = request.Image;
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
