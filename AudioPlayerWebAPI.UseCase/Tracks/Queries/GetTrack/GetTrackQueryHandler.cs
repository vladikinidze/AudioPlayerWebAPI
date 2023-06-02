using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTrack
{
    public class GetTrackQueryHandler : IRequestHandler<GetTrackQuery, TrackViewModel>
    {
        private readonly IAudioPlayerDbContext _context;
        private readonly IMapper _mapper;

        public GetTrackQueryHandler(IAudioPlayerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TrackViewModel> Handle(GetTrackQuery request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlists
                .FirstOrDefaultAsync(p => p.Id == request.PlaylistId, cancellationToken);

            if (playlist != null)
            {
                if (playlist.Private)
                {
                    var userPlaylist = await _context.UserPlaylists
                        .FirstOrDefaultAsync(up => up.PlaylistId == playlist.Id
                                                   && up.UserId == request.UserId, cancellationToken);
                    if (userPlaylist == null)
                    {
                        throw new NotFoundException(nameof(Playlist), request.PlaylistId.ToString());
                    }
                }

                var track = await _context.PlaylistTracks
                    .Where(pt => pt.PlaylistId == playlist.Id)
                    .ProjectTo<TrackViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                return track ?? throw new NotFoundException(nameof(Track), request.Id.ToString());
            }

            throw new NotFoundException(nameof(Playlist), request.PlaylistId.ToString());
        }
    }
}
