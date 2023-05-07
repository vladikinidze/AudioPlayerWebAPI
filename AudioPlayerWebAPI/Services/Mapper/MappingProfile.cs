namespace AudioPlayerWebAPI.Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Track, TrackDto>().ReverseMap();
        }
    }
}
