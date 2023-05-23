using AutoMapper;
using System.ComponentModel.DataAnnotations;
using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.CreateTrack;

namespace AudioPlayer.WebAPI.Models
{
    public class CreateTrackDto : IMap<CreateTrackCommand>
    {
        [Required]
        public Guid PlaylistId { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Text { get; set; }
        public bool Explicit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateTrackDto, CreateTrackCommand>()
                .ForMember(trackCommand => trackCommand.Title,
                    opt => opt.MapFrom(playlistDto => playlistDto.Title))
                .ForMember(trackCommand => trackCommand.PlaylistId,
                    opt => opt.MapFrom(playlistDto => playlistDto.PlaylistId))
                .ForMember(trackCommand => trackCommand.Text,
                    opt => opt.MapFrom(playlistDto => playlistDto.Text))
                .ForMember(trackCommand => trackCommand.Explicit,
                    opt => opt.MapFrom(playlistDto => playlistDto.Explicit));
        }
    }
}
