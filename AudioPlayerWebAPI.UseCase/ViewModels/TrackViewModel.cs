using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Mapping;
using AutoMapper;

namespace AudioPlayerWebAPI.UseCase.ViewModels
{
    public class TrackViewModel : IMap<Track>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public double Duration { get; set; }
        public string Audio { get; set; } = null!;
        public bool Explicit { get; set; }
        public Guid PlaylistId { get; set; }
        public DateTime AddedDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Track, TrackViewModel>()
                .ForMember(trackViewModel => trackViewModel.Id,
                    opt => opt.MapFrom(track => track.Id))
                .ForMember(trackViewModel => trackViewModel.PlaylistId,
                    opt => opt.MapFrom(track => track.Playlists.
                        First(p => p.TrackId == Id && p.IsParent).PlaylistId))
                .ForMember(trackViewModel => trackViewModel.Title,
                    opt => opt.MapFrom(track => track.Title))
                .ForMember(trackViewModel => trackViewModel.Audio,
                    opt => opt.MapFrom(track => track.Audio))
                .ForMember(trackViewModel => trackViewModel.Duration,
                    opt => opt.MapFrom(track => track.Duration))
                .ForMember(trackViewModel => trackViewModel.Explicit,
                    opt => opt.MapFrom(track => track.Explicit))
                .ForMember(trackViewModel => trackViewModel.AddedDate,
                    opt => opt.MapFrom(track => track.AddedDate));
        }
    }
}
