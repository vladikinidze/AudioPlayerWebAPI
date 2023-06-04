using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTrack;
using AutoMapper;

namespace AudioPlayerWebAPI.Models;

public class GetTrackDto: IMap<GetTrackQuery>
{
    public Guid TrackId { get; set; }
    public Guid PlaylistId { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<GetTrackDto, GetTrackQuery>()
            .ForMember(query => query.Id,
                opt => opt.MapFrom(trackDto => trackDto.TrackId))
            .ForMember(query => query.PlaylistId,
                opt => opt.MapFrom(trackDto => trackDto.PlaylistId));
    }
}
