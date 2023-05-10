namespace AudioPlayerWebAPI.Models.DTO
{
    public class PlaylistUserDto : PlaylistDto
    {
        public List<Track> Tracks { get; set; } = new();
    }
}
