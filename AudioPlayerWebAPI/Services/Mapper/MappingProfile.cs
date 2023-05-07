namespace AudioPlayerWebAPI.Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, RegisterDto>().ReverseMap();
            CreateMap<Track, TrackDto>();
            CreateMap<Playlist, PlaylistDto>();
        }
    }
}
