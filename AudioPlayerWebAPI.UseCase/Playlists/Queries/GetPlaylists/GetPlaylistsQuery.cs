using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylists
{
    public class GetPlaylistsQuery : IRequest<PlaylistListViewModel>
    { }
}
