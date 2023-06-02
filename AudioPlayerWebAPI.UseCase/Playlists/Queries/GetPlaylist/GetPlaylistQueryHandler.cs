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

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylist
{
    public class GetPlaylistQueryHandler : IRequestHandler<GetPlaylistQuery, PlaylistViewModel>
    {
        private readonly IAudioPlayerDbContext _context;
        private readonly IMapper _mapper;

        public GetPlaylistQueryHandler(IAudioPlayerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PlaylistViewModel> Handle(GetPlaylistQuery request, CancellationToken cancellationToken)
        {
            var playlistTrack = await _context.UserPlaylists
                .Include(userPlaylists => userPlaylists.Playlist.Users)
                .Include(userPlaylists => userPlaylists.Playlist.Tracks)
                .FirstOrDefaultAsync(userPlaylists => userPlaylists.PlaylistId == request.Id, cancellationToken);

            if (playlistTrack == null)
            {
                throw new NotFoundException(nameof(Playlist), request.Id.ToString());
            }
            
            var vm = _mapper.Map<PlaylistViewModel>(playlistTrack);
            var user = playlistTrack!.Playlist.Users.First(user => user.IsOwner && user.PlaylistId == vm.Id);
            vm.UserId = user.UserId;
            vm.Username = _context.Users.First(x => x.Id == user.UserId).Username;
            vm.IsOwner = vm.UserId == request.UserId;
            vm.Tracks = await _context.PlaylistTracks
                .Where(x => x.PlaylistId == vm.Id)
                .ProjectTo<TrackDto>(_mapper.ConfigurationProvider)
                .OrderBy(x => x.AddedDate)
                .ToListAsync(cancellationToken);

            if (!vm.Private) return vm;
            if (request.UserId == null || vm.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Playlist), request.Id.ToString());
            }
            return vm;
        }
    }
}