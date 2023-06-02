using AudioPlayerWebAPI.UseCase.Dtos;

namespace AudioPlayerWebAPI.UseCase.ViewModels
{
    public class TrackListViewModel
    {
        public IList<TrackDto> Tracks { get; set; } = null!;
    }
}
