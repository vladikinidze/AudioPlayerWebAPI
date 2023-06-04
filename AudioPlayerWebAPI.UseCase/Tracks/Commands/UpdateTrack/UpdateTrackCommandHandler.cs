using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.UpdateTrack
{
    public class UpdateTrackCommandHandler :
        IRequestHandler<UpdateTrackCommand, Unit>
    {
        private readonly IAudioPlayerDbContext _context;

        public UpdateTrackCommandHandler(IAudioPlayerDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
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
                    if (request.Audio != null)  
                    {
                        var audio = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files\\Audio",
                            playlistTracks.Track.Audio!);
                        if (File.Exists(audio))
                        {
                            File.Delete(audio);
                        }
                        playlistTracks.Track.Audio = request.Audio;
                    }
                    playlistTracks.Track.Title = request.Title;
                    playlistTracks.Track.Duration = request.Duration;
                    playlistTracks.Track.Explicit = request.Explicit;
                    await _context.SaveChangesAsync(cancellationToken);
                    return Unit.Value;
                }
            }

            throw new NotFoundException(nameof(Track), request.Id.ToString());
        }
    }
}