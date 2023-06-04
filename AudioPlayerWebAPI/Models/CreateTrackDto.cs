using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.CreateTrack;
using AutoMapper;

namespace AudioPlayerWebAPI.Models
{
    public class CreateTrackDto : IMap<CreateTrackCommand>
    {
        public Guid? PlaylistId { get; set; }
        public string? Title { get; set; } = null!;
        public IFormFile? Audio { get; set; } = null!;
        public bool? Explicit { get; set; }
        public double Duration { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateTrackDto, CreateTrackCommand>()
                .ForMember(trackCommand => trackCommand.Title,
                    opt => opt.MapFrom(playlistDto => playlistDto.Title))
                .ForMember(trackCommand => trackCommand.PlaylistId,
                    opt => opt.MapFrom(playlistDto => playlistDto.PlaylistId))
                .ForMember(trackCommand => trackCommand.Duration,
                    opt => opt.MapFrom(playlistDto => playlistDto.Duration))
                .ForMember(trackCommand => trackCommand.Explicit,
                    opt => opt.MapFrom(playlistDto => playlistDto.Explicit));
        }
    }
}
