using MediatR;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.IsAddedPlaylist;

public class IsAddedPlaylistQuery : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid PlaylistId { get; set; }
}