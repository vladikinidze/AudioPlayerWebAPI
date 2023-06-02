using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.UpdateTrack;
using AutoMapper;

namespace AudioPlayerWebAPI.Models
{
    public class UpdateTrackDto : IMap<UpdateTrackCommand>
    {
        public Guid? Id { get; set; }
        public Guid? PlaylistId { get; set; }
        public string? Title { get; set; } = null!;
        public IFormFile? Audio { get; set; } = null!;
        public bool? Explicit { get; set; }
        public string? Text { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateTrackDto, UpdateTrackCommand>()
                .ForMember(trackCommand => trackCommand.Id,
                    opt => opt.MapFrom(playlistDto => playlistDto.Id))
                .ForMember(trackCommand => trackCommand.PlaylistId,
                    opt => opt.MapFrom(playlistDto => playlistDto.PlaylistId))
                .ForMember(trackCommand => trackCommand.Title,
                    opt => opt.MapFrom(playlistDto => playlistDto.Title))
                .ForMember(trackCommand => trackCommand.Text,
                    opt => opt.MapFrom(playlistDto => playlistDto.Text))
                .ForMember(trackCommand => trackCommand.Explicit,
                    opt => opt.MapFrom(playlistDto => playlistDto.Explicit));
        }
    }
}
