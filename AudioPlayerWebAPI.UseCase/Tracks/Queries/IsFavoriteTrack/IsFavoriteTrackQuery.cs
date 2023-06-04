using MediatR;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.IsFavoriteTrack;

public class IsFavoriteTrackQuery : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid TrackId { get; set; }
}