using System.ComponentModel.DataAnnotations;
using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.UpdatePlaylist;
using AutoMapper;

namespace AudioPlayerWebAPI.Models
{
    public class UpdatePlaylistDto : IMap<UpdatePlaylistCommand>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public bool Private { get; set; }
        public IFormFile? Image { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePlaylistDto, UpdatePlaylistCommand>()
                .ForMember(playlistCommand => playlistCommand.Id,
                    opt => opt.MapFrom(playlistDto => playlistDto.Id))
                .ForMember(playlistCommand => playlistCommand.Title,
                    opt => opt.MapFrom(playlistDto => playlistDto.Title));
        }
    }
}
