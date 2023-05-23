using MediatR;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.CreatePlaylist
{
    public class CreatePlaylistCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string? Image { get; set; }
        public bool Private { get; set; }
    }
}
