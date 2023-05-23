using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteTrack
{
    public class DeleteTrackCommandHandler : IRequestHandler<DeleteTrackCommand, Unit>
    {
        private readonly IAudioPlayerDbContext _context;

        public DeleteTrackCommandHandler(IAudioPlayerDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTrackCommand request, CancellationToken cancellationToken)
        {
            var playlistTracks = await _context.PlaylistTracks
                .Include(pt => pt.Track)
                .FirstOrDefaultAsync(pt => pt.PlaylistId == request.PlaylistId
                                           && pt.TrackId == request.Id, cancellationToken);

            if (playlistTracks != null)
            {
                var userPlaylists = await _context.UserPlaylists
                    .FirstOrDefaultAsync(up => up.PlaylistId == playlistTracks.PlaylistId
                                               && up.UserId == request.UserId, cancellationToken);

                if (userPlaylists is { IsOwner: true })
                {
                    _context.Tracks.Remove(playlistTracks.Track);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Unit.Value;
                }
            }

            throw new NotFoundException(nameof(Track), request.Id.ToString());
        }
    }
}
