using AudioPlayerWebAPI.UseCase.Dtos;
using AudioPlayerWebAPI.UseCase.Interfaces;
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

        public async Task<PlaylistListViewModel> Handle(GetPlaylistsQuery request, CancellationToken cancellationToken)
        {
            var playlists = await _context.UserPlaylists
                .ProjectTo<PlaylistDto>(_mapper.ConfigurationProvider)
                .Where(p => !p.Private)
                .ToListAsync(cancellationToken);

            return new PlaylistListViewModel { Playlists = playlists };
        }
    }
}
