using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Dtos;
using AudioPlayerWebAPI.UseCase.Mapping;
using AutoMapper;

namespace AudioPlayerWebAPI.UseCase.ViewModels
{
    public class PlaylistViewModel : IMap<Playlist>
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Image { get; set; }
        public bool IsOwner { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Private { get; set; }
        public ICollection<TrackDto> Tracks { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Playlist, PlaylistViewModel>()
                .ForMember(playlistViewModel => playlistViewModel.Id,
                    opt => opt.MapFrom(playlist => 
                        playlist.Id))
                .ForMember(playlistViewModel => playlistViewModel.Title,
                    opt => opt.MapFrom(playlist => 
                        playlist.Title))
                .ForMember(playlistViewModel => playlistViewModel.Private,
                    opt => opt.MapFrom(playlist => 
                        playlist.Private))
                .ForMember(playlistViewModel => playlistViewModel.Image,
                    opt => opt.MapFrom(playlist => 
                        playlist.Image))
                .ForMember(playlistViewModel => playlistViewModel.CreationDate,
                    opt => opt.MapFrom(playlist => 
                        playlist.CreationDate));
            
            profile.CreateMap<UserPlaylists, PlaylistViewModel>()
                .ForMember(playlistViewModel => playlistViewModel.Id,
                    opt => opt.MapFrom(playlist => 
                    playlist.PlaylistId))
                .ForMember(playlistViewModel => playlistViewModel.Title,
                    opt => opt.MapFrom(playlist => 
                    playlist.Playlist.Title))
                .ForMember(playlistViewModel => playlistViewModel.Private,
                    opt => opt.MapFrom(playlist => 
                    playlist.Playlist.Private))
                .ForMember(playlistViewModel => playlistViewModel.Image,
                    opt => opt.MapFrom(playlist => 
                    playlist.Playlist.Image))
                .ForMember(playlistViewModel => playlistViewModel.CreationDate,
                    opt => opt.MapFrom(playlist => 
                    playlist.Playlist.CreationDate));
        }
    }
}
