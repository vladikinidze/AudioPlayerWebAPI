using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Dtos;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GetFavoritePlaylist;

public class GetFavoritePlaylistQueryHandler : IRequestHandler<GetFavoritePlaylistQuery, PlaylistViewModel>
{
    private readonly IAudioPlayerDbContext _context;
    private readonly IMapper _mapper;

    public GetFavoritePlaylistQueryHandler(IAudioPlayerDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PlaylistViewModel> Handle(GetFavoritePlaylistQuery request, CancellationToken cancellationToken)
    {
        var userPlaylist = await _context.UserPlaylists
            .Include(x => x.Playlist)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId 
                                      && x.Playlist.Title == "Favorite", cancellationToken);
        if (userPlaylist == null)
        {
            throw new NotFoundException(nameof(Playlist), "favorite");
        }
        var vm = _mapper.Map<PlaylistViewModel>(userPlaylist);
        vm.UserId = userPlaylist.User.Id;
        vm.Username = userPlaylist.User.Username;
        vm.IsOwner = vm.UserId == request.UserId;
        vm.Tracks = await _context.PlaylistTracks
            .Where(x => x.PlaylistId == vm.Id)
            .ProjectTo<TrackDto>(_mapper.ConfigurationProvider)
            .OrderByDescending(x => x.AddedDate)
            .ToListAsync(cancellationToken);
        return vm;
    }
}