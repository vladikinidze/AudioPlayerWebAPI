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
                .ForMember(playlistViewModel => playlistViewModel.Id,
                    opt => opt.MapFrom(playlist => playlist.Id))
                .ForMember(playlistViewModel => playlistViewModel.Title,
                    opt => opt.MapFrom(playlist => playlist.Title))
                .ForMember(playlistViewModel => playlistViewModel.Private,
                    opt => opt.MapFrom(playlist => playlist.Private))
                .ForMember(playlistViewModel => playlistViewModel.Image,
                    opt => opt.MapFrom(playlist => playlist.Image))
                .ForMember(playlistViewModel => playlistViewModel.Tracks,
                    opt => opt.MapFrom(playlist => playlist.Tracks.Select(x => x.Track).ToList()))
                .ForMember(playlistViewModel => playlistViewModel.CreationDate,
                    opt => opt.MapFrom(playlist => playlist.CreationDate));
        }
    }
}
