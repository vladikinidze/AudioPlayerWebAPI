using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracks;

public class GetTracksQuery : IRequest<TrackListViewModel>
{
    public Guid? UserId { get; set; }
}