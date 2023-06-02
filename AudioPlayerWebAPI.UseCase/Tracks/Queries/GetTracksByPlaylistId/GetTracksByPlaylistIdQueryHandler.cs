using System.Diagnostics.CodeAnalysis;
using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Dtos;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracksByPlaylistId
{
    public class GetTracksByPlaylistIdQueryHandler : IRequestHandler<GetTracksByPlaylistIdQuery, TrackListViewModel>
    {
        private readonly IAudioPlayerDbContext _context;
        private readonly IMapper _mapper;

        public GetTracksByPlaylistIdQueryHandler(IAudioPlayerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<TrackListViewModel> Handle(GetTracksByPlaylistIdQuery request, CancellationToken cancellationToken)
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
                    .OrderBy(t => t.AddedTime)
                    .ProjectTo<TrackDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return new TrackListViewModel { Tracks = tracks };
            }

            throw new NotFoundException(nameof(Playlist), request.PlaylistId.ToString());
        }
    }
}
