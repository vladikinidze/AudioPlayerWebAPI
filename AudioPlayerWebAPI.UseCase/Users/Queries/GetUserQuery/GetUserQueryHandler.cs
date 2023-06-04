using System.Diagnostics.CodeAnalysis;
using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Users.Queries.GetUserQuery;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserViewModel>
{
    private readonly IAudioPlayerDbContext _context;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IAudioPlayerDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<UserViewModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken);

        if (user != null)
        {
            var playlist = await _context.UserPlaylists
                .Include(x => x.Playlist)
                .FirstOrDefaultAsync(x => x.UserId == user.Id 
                                          && x.Playlist.Title == "Favorite", cancellationToken);
            if (playlist == null)
            {
                throw new NotFoundException(nameof(Playlist), "playlist");
            }
            user.FavoritePlaylistId = playlist!.PlaylistId;
            return user;
        }
        throw new NotFoundException(nameof(User), request.Id.ToString());
    }
}