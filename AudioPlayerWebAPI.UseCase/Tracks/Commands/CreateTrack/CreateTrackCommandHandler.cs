using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.CreateTrack
{
    public class CreateTrackCommandHandler 
        : IRequestHandler<CreateTrackCommand, Guid>
    {
        private readonly IAudioPlayerDbContext _context;

        public CreateTrackCommandHandler(IAudioPlayerDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateTrackCommand request, 
            CancellationToken cancellationToken)
        {
            var userPlaylist = await _context.UserPlaylists
                .Include(up => up.Playlist)
                .FirstOrDefaultAsync(up => up.PlaylistId == request.PlaylistId
                                           && up.UserId == request.UserId, cancellationToken);

            if (userPlaylist is not { IsOwner: true })
            {
                throw new Exception($"You cannot add a track to this playlist.");
            }

            var track = new Track()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Text = request.Text,
                Explicit = request.Explicit,
                AddedDate = DateTime.Now,
            };

            var playlistTracks = new PlaylistTracks()
            {
                AddedTime = DateTime.Now,
                IsParent = true,
                Playlist = userPlaylist.Playlist,
                PlaylistId = userPlaylist.PlaylistId,
                Track = track,
                TrackId = track.Id,
            };

            await _context.Tracks.AddAsync(track, cancellationToken);
            await _context.PlaylistTracks.AddAsync(playlistTracks, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return track.Id;
        }
    }
}
