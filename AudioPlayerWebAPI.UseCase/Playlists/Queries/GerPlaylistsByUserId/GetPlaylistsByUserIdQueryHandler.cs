using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioPlayerWebAPI.UseCase.Dtos;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GerPlaylistsByUserId
{
    public class GetPlaylistsByUserIdQueryHandler : IRequestHandler<GetPlaylistsByUserIdQuery, PlaylistListViewModel>
    {
        private readonly IAudioPlayerDbContext _context;
        private readonly IMapper _mapper;

        public GetPlaylistsByUserIdQueryHandler(IAudioPlayerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PlaylistListViewModel> Handle(GetPlaylistsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var playlists = await _context.UserPlaylists
                .Where(p => p.UserId == request.UserId)
                .ProjectTo<PlaylistDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PlaylistListViewModel { Playlists = playlists };
        }
    }
}
