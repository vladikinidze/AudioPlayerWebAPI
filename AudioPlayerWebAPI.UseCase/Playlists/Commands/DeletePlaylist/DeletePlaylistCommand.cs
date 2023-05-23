using MediatR;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.DeletePlaylist
{
    public class DeletePlaylistCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
