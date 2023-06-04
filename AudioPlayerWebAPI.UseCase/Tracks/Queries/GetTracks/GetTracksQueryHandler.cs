using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Dtos;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracks;

public class GetTracksQueryHandler : IRequestHandler<GetTracksQuery, TrackListViewModel>
{
    private readonly IAudioPlayerDbContext _context;
    private readonly IMapper _mapper;

    public GetTracksQueryHandler(IAudioPlayerDbContext context, IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    private bool NeedInclude(PlaylistTracks playlistTracks, GetTracksQuery request)
    {
        return !playlistTracks.Playlist.Private || 
               request.UserId != null && playlistTracks.Playlist.Users.Any(x => x.UserId == request.UserId);
    }

    public async Task<TrackListViewModel> Handle(GetTracksQuery request, CancellationToken cancellationToken)
    {
        var playlistTracks = await _context.PlaylistTracks
            .Include(x => x.Playlist)
            .Include(x => x.Playlist.Users)
            .Include(x => x.Track)
            .Where(x => x.Playlist.Title != "Favorite")
            .ToListAsync(cancellationToken);
        playlistTracks = playlistTracks.Where(x => NeedInclude(x, request)).ToList();
        return new TrackListViewModel { Tracks = _mapper.Map<List<TrackDto>>(playlistTracks) };
    }
}