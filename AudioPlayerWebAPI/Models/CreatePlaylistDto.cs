using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.CreatePlaylist;
using AutoMapper;

namespace AudioPlayerWebAPI.Models
{
    public class CreatePlaylistDto : IMap<CreatePlaylistCommand>
    {
        public string? Title { get; set; }
        public bool? Private { get; set; } = false;
        public IFormFile? Image { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePlaylistDto, CreatePlaylistCommand>()
                .ForMember(playlistCommand => playlistCommand.Title,
                    opt => opt.MapFrom(playlistDto => playlistDto.Title))
                .ForMember(playlistCommand => playlistCommand.Private,
                    opt => opt.MapFrom(playlistDto => playlistDto.Private));
        }
    }
}
