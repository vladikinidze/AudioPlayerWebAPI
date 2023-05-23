using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Dtos;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracks
{
    public class GetTracksQueryHandler : IRequestHandler<GetTracksQuery, TrackListViewModel>
    {
        private readonly IAudioPlayerDbContext _context;
        private readonly IMapper _mapper;

        public GetTracksQueryHandler(IAudioPlayerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TrackListViewModel> Handle(GetTracksQuery request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlists
                .FirstOrDefaultAsync(p => p.Id == request.PlaylistId, cancellationToken);

            if (playlist != null)
            {
                if (playlist.Private)
                {
                    if (request.UserId != null)
                    {
                        var userPlaylist = await _context.UserPlaylists
                            .FirstOrDefaultAsync(up => up.PlaylistId == playlist.Id
                                                       && up.UserId == request.UserId, cancellationToken);
                        if (userPlaylist == null)
                        {
                            throw new NotFoundException(nameof(Playlist), request.PlaylistId.ToString());
                        }
                    }
                    else
                    {
                        throw new NotFoundException(nameof(Playlist), request.PlaylistId.ToString());
                    }
                }

                var tracks = await _context.PlaylistTracks
                    .Where(pt => pt.PlaylistId == playlist.Id)
                    .OrderByDescending(t => t.AddedTime)
                    .ProjectTo<TrackDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return new TrackListViewModel { Tracks = tracks };
            }

            throw new NotFoundException(nameof(Playlist), request.PlaylistId.ToString());
        }
    }
}
