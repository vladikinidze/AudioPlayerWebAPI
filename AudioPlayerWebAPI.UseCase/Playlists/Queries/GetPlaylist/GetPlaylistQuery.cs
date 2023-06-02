using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylist
{
    public class GetPlaylistQuery : IRequest<PlaylistViewModel>
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Payload { get; set; }
    }
}
