using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.CreatePlaylist
{
    public class CreatePlaylistCommandHandler
        : IRequestHandler<CreatePlaylistCommand, Guid>
    {
        private readonly IAudioPlayerDbContext _context;

        public CreatePlaylistCommandHandler(IAudioPlayerDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreatePlaylistCommand request,
            CancellationToken cancellationToken)
        {
            var playlist = new Playlist()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Image = request.Image,
                CreationDate = DateTime.Now,
                Private = request.Private,
            };

            var userPlaylist = new UserPlaylists()
            {
                AddedDate = DateTime.Now,
                IsOwner = true,
                PlaylistId = playlist.Id,
                Playlist = playlist,
                UserId = request.UserId,
            };

            await _context.Playlists.AddAsync(playlist, cancellationToken);
            await _context.UserPlaylists.AddAsync(userPlaylist, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return playlist.Id;
        }
    }
}
