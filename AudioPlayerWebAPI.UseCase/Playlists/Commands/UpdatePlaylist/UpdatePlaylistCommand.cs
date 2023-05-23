using MediatR;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.UpdatePlaylist
{
    public class UpdatePlaylistCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string? Image { get; set; }
    }
}
