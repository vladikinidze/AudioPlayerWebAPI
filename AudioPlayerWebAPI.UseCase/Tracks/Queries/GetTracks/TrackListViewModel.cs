using AudioPlayerWebAPI.UseCase.Dtos;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracks
{
    public class TrackListViewModel
    {
        public IList<TrackDto> Tracks { get; set; }
    }
}
