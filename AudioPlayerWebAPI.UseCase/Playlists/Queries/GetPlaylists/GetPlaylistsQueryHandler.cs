using System.Diagnostics.CodeAnalysis;
using AudioPlayerWebAPI.UseCase.Dtos;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylists
{
    public class GetPlaylistsQueryHandler : IRequestHandler<GetPlaylistsQuery, PlaylistListViewModel>
    {
        private readonly IAudioPlayerDbContext _context;
        private readonly IMapper _mapper;

        public GetPlaylistsQueryHandler(IAudioPlayerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private bool NeedInclude(PlaylistDto playlistDto, GetPlaylistsQuery request)
        {
            return !playlistDto.Private || request.UserId != null && playlistDto.UserId == request.UserId;
        }
        
        public async Task<PlaylistListViewModel> Handle(GetPlaylistsQuery request, CancellationToken cancellationToken)
        {
            var playlists = await _context.UserPlaylists
                .Where(x => x.IsOwner == true)
                .OrderByDescending(userPlaylists => userPlaylists.AddedDate)
                .ProjectTo<PlaylistDto>(_mapper.ConfigurationProvider)
                .Where(x => x.Title != "Favorite")
                .ToListAsync(cancellationToken);
            
            playlists = playlists.Where(playlist => NeedInclude(playlist, request)).ToList();

            playlists.ForEach(playlist =>
            {
                playlist.Tracks = _context.PlaylistTracks
                    .Where(tracks => tracks.PlaylistId == playlist.Id)
                    .ProjectTo<TrackDto>(_mapper.ConfigurationProvider)
                    .OrderBy(x => x.AddedDate)
                    .ToList();
            });

            return new PlaylistListViewModel { Playlists = playlists};
        }
    }
}
