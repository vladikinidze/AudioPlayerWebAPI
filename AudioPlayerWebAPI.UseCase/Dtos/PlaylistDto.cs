using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Mapping;
using AutoMapper;

namespace AudioPlayerWebAPI.UseCase.Dtos
{
    public class PlaylistDto : IMap<Playlist>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Image { get; set; }
        public bool Private { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid UserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Playlist, PlaylistDto>()
                .ForMember(pVm => pVm.Id,
                    opt => opt.MapFrom(p => p.Id))
                .ForMember(pVm => pVm.UserId,
                    opt => opt.MapFrom(p => p.Users))
                .ForMember(pVm => pVm.Title,
                    opt => opt.MapFrom(p => p.Title))
                .ForMember(pVm => pVm.Image,
                    opt => opt.MapFrom(p => p.Image))
                .ForMember(pVm => pVm.Private,
                    opt => opt.MapFrom(p => p.Private))
                .ForMember(pVm => pVm.CreationDate,
                    opt => opt.MapFrom(p => p.CreationDate));
        }
    }
}
