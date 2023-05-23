using System.ComponentModel.DataAnnotations;
using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.CreatePlaylist;
using AutoMapper;

namespace AudioPlayerWebAPI.Models
{
    public class CreatePlaylistDto : IMap<CreatePlaylistCommand>
    {
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePlaylistDto, CreatePlaylistCommand>()
                .ForMember(playlistCommand => playlistCommand.Title,
                    opt => opt.MapFrom(playlistDto => playlistDto.Title));
        }
    }
}
