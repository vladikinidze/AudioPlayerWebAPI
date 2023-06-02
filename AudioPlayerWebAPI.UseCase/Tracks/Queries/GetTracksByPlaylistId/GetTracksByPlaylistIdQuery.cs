using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracksByPlaylistId
{
    public class GetTracksByPlaylistIdQuery : IRequest<TrackListViewModel>
    {
        public Guid PlaylistId { get; set; }
        public Guid? UserId { get; set; }
    }
}
