using MediatR;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteFromFavoritePlaylist;

public class DeleteFromFavoritePlaylistCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid TrackId { get; set; }
}