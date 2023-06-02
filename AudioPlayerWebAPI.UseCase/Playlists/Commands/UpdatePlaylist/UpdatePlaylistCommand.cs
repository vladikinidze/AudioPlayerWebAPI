using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Dtos;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.UpdatePlaylist
{
    public class UpdatePlaylistCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = null!;
        public string? Image { get; set; }
        public bool Private { get; set; }
    }
}
