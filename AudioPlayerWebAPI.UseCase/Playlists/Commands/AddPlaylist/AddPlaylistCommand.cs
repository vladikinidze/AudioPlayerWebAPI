using MediatR;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.AddPlaylist;

public class AddPlaylistCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid PlaylistId { get; set; }
}