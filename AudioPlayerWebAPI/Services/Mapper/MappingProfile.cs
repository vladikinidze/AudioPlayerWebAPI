namespace AudioPlayerWebAPI.Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Track, TrackDto>().ReverseMap();
            CreateMap<Playlist, PlaylistDto>().ReverseMap();
            CreateMap<Playlist, PlaylistUserDto>().ReverseMap();
        }
    }
}
