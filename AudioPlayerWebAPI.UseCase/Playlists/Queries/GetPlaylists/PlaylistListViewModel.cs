using AudioPlayerWebAPI.UseCase.Dtos;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylists
{
    public class PlaylistListViewModel
    {
        public IList<PlaylistDto> Playlists { get; set; }
    }
}
