using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTrack
{
    public class GetTrackQuery : IRequest<TrackViewModel>
    {
        public Guid Id { get; set; }
        public Guid PlaylistId { get; set; }
        public Guid UserId { get; set; }
    }
}
