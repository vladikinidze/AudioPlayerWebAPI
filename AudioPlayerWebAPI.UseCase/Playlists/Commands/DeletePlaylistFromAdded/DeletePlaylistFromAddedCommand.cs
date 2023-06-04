using MediatR;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.DeletePlaylistFromAdded;

public class DeletePlaylistFromAddedCommand: IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid PlaylistId { get; set; }
}