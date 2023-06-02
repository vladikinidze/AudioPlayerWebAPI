using AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteTrack;
using AutoMapper;

namespace AudioPlayerWebAPI.Models
{
    public class DeleteTrackDto
    {
        public Guid Id { get; set; }
        public Guid PlaylistId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteTrackDto, DeleteTrackCommand>()
                .ForMember(deleteTrackCommand => deleteTrackCommand.PlaylistId,
                    opt => opt.MapFrom(deleteTrackDto => deleteTrackDto.PlaylistId))
                .ForMember(deleteTrackCommand => deleteTrackCommand.Id,
                    opt => opt.MapFrom(deleteTrackDto => deleteTrackDto.Id));
        }
    }
}
