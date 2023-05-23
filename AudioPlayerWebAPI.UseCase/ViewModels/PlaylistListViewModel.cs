using AudioPlayerWebAPI.UseCase.Dtos;

namespace AudioPlayerWebAPI.UseCase.ViewModels
{
    public class PlaylistListViewModel
    {
        public IList<PlaylistDto> Playlists { get; set; }
    }
}
