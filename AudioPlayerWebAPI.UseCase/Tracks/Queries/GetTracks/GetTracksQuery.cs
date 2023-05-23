using MediatR;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracks
{
    public class GetTracksQuery : IRequest<TrackListViewModel>
    {
        public Guid PlaylistId { get; set; }
        public Guid? UserId { get; set; }
    }
}
