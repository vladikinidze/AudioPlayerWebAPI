using MediatR;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.AddToFavoritePlaylist;

public class AddToFavoritePlaylistCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid TrackId { get; set; }
}