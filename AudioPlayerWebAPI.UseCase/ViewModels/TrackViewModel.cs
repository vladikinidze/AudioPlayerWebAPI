using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Mapping;
using AutoMapper;

namespace AudioPlayerWebAPI.UseCase.ViewModels
{
    public class TrackViewModel : IMap<Track>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Text { get; set; }
        public string Audio { get; set; }
        public bool Explicit { get; set; }
        public DateTime AddedDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Track, TrackViewModel>()
                .ForMember(tVm => tVm.Id,
                    opt => opt.MapFrom(p => p.Id))
                .ForMember(tVm => tVm.Title,
                    opt => opt.MapFrom(p => p.Title))
                .ForMember(tVm => tVm.Audio,
                    opt => opt.MapFrom(p => p.Audio))
                .ForMember(tVm => tVm.Text,
                    opt => opt.MapFrom(p => p.Text))
                .ForMember(tVm => tVm.Explicit,
                    opt => opt.MapFrom(p => p.Explicit))
                .ForMember(tVm => tVm.AddedDate,
                    opt => opt.MapFrom(p => p.AddedDate));
        }
    }
}
