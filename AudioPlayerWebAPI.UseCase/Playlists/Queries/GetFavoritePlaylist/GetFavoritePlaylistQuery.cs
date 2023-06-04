using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GetFavoritePlaylist;

public class GetFavoritePlaylistQuery: IRequest<PlaylistViewModel>
{
    public Guid UserId { get; set; }
}