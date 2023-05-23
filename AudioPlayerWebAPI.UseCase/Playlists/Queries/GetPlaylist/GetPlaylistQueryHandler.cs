using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
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
            var userPlaylist = await _context.UserPlaylists
                .Include(up => up.Playlist)
                .FirstOrDefaultAsync(up => up.PlaylistId == request.Id, cancellationToken);


            if (userPlaylist != null)
            {
                if (!userPlaylist.Playlist.Private)
                {
                    return _mapper.Map<PlaylistViewModel>(userPlaylist.Playlist);
                }

                if (request.UserId == null || userPlaylist.UserId != request.UserId)
                {
                    throw new NotFoundException(nameof(Playlist), request.Id.ToString());
                }

                var vm = _mapper.Map<PlaylistViewModel>(userPlaylist.Playlist);
                vm.UserId = userPlaylist.UserId;
                return vm;
            }

            throw new NotFoundException(nameof(Playlist), request.Id.ToString());
        }
    }
}
