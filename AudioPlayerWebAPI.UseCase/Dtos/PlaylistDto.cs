using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Mapping;
using AutoMapper;

namespace AudioPlayerWebAPI.UseCase.Dtos
{
    public class PlaylistDto : IMap<UserPlaylists>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Image { get; set; }
        public bool Private { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<TrackDto> Tracks { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserPlaylists, PlaylistDto>()
                .ForMember(pVm => pVm.Id,
                    opt => opt.MapFrom(p => p.Playlist.Id))
                .ForMember(pVm => pVm.UserId,
                    opt => opt.MapFrom(p => p.UserId))
                .ForMember(pVm => pVm.Username,
                    opt => opt.MapFrom(p => p.User.Username))
                .ForMember(pVm => pVm.Title,
                    opt => opt.MapFrom(p => p.Playlist.Title))
                .ForMember(pVm => pVm.Image,
                    opt => opt.MapFrom(p => p.Playlist.Image))
                .ForMember(pVm => pVm.Private,
                    opt => opt.MapFrom(p => p.Playlist.Private))
                .ForMember(pVm => pVm.CreationDate,
                    opt => opt.MapFrom(p => p.Playlist.CreationDate));
        }
    }
}
