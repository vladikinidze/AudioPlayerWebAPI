using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GerPlaylistsByUserId
{
    public class GetPlaylistsByUserIdQuery : IRequest<PlaylistListViewModel>
    {
        public Guid UserId { get; set; }
    }
}
