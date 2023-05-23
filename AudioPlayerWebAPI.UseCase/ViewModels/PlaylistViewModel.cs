using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Mapping;
using AutoMapper;

namespace AudioPlayerWebAPI.UseCase.ViewModels
{
    public class PlaylistViewModel : IMap<Playlist>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string? Image { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Private { get; set; }
        public ICollection<TrackViewModel> Tracks { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Playlist, PlaylistViewModel>()
                .ForMember(pVm => pVm.Id,
                    opt => opt.MapFrom(p => p.Id))
                .ForMember(pVm => pVm.Title,
                    opt => opt.MapFrom(p => p.Title))
                .ForMember(pVm => pVm.Private,
                    opt => opt.MapFrom(p => p.Private))
                .ForMember(pVm => pVm.Image,
                    opt => opt.MapFrom(p => p.Image))
                .ForMember(pVm => pVm.Tracks,
                    opt => opt.MapFrom(p => p.Tracks.Select(x => x.Track).ToList()))
                .ForMember(pVm => pVm.CreationDate,
                    opt => opt.MapFrom(p => p.CreationDate));
        }
    }
}
