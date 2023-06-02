using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Dtos;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GerPlaylistsByUserId
{
    public class GetPlaylistsByUserIdQueryHandler 
        : IRequestHandler<GetPlaylistsByUserIdQuery, PlaylistListViewModel>
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
            var user = await _context.Users
                .FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

            if (user != null)
            {
                var playlists = await _context.UserPlaylists
                    .ProjectTo<PlaylistDto>(_mapper.ConfigurationProvider)
                    .Where(p => p.UserId == user.Id && p.Title != "Favorite")
                    .ToListAsync(cancellationToken);
                return new PlaylistListViewModel { Playlists = playlists };
            }

            throw new NotFoundException(nameof(User), request.UserId.ToString());
        }
    }
}
