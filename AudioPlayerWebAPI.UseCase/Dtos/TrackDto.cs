using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Mapping;
using AutoMapper;

namespace AudioPlayerWebAPI.UseCase.Dtos
{
    public class TrackDto : IMap<Track>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public double Duration { get; set; }
        public string Audio { get; set; } = null!;
        public bool Explicit { get; set; }
        public DateTime AddedDate { get; set; }
        public Guid PlaylistId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Track, TrackDto>()
                .ForMember(trackDto => trackDto.Id,
                    opt => opt.MapFrom(track => track.Id))
                .ForMember(trackDto => trackDto.PlaylistId,
                    opt => opt.MapFrom(track => 
                        track.Playlists.First(tracks => tracks.TrackId == Id && tracks.IsParent).PlaylistId))
                .ForMember(trackDto => trackDto.Title,
                    opt => opt.MapFrom(track => track.Title))
                .ForMember(trackDto => trackDto.Audio,
                    opt => opt.MapFrom(track => track.Audio))
                .ForMember(trackDto => trackDto.Duration,
                    opt => opt.MapFrom(track => track.Duration))
                .ForMember(trackDto => trackDto.Explicit,
                    opt => opt.MapFrom(track => track.Explicit))
                .ForMember(trackDto => trackDto.AddedDate,
                    opt => opt.MapFrom(track => track.AddedDate));

            profile.CreateMap<PlaylistTracks, TrackDto>()
                .ForMember(trackDto => trackDto.Id,
                    opt => opt.MapFrom(playlistTracks => playlistTracks.TrackId))
                .ForMember(trackDto => trackDto.PlaylistId,
                    opt => opt.MapFrom(playlistTracks => playlistTracks.PlaylistId))
                .ForMember(trackDto => trackDto.Title,
                    opt => opt.MapFrom(playlistTracks => playlistTracks.Track.Title))
                .ForMember(trackDto => trackDto.Duration,
                    opt => opt.MapFrom(playlistTracks => playlistTracks.Track.Duration))
                .ForMember(trackDto => trackDto.AddedDate,
                    opt => opt.MapFrom(playlistTracks => playlistTracks.Track.AddedDate))
                .ForMember(trackDto => trackDto.Audio,
                    opt => opt.MapFrom(playlistTracks => playlistTracks.Track.Audio))
                .ForMember(trackDto => trackDto.Explicit,
                    opt => opt.MapFrom(playlistTracks => playlistTracks.Track.Explicit));
        }
    }
}
