namespace AudioPlayerWebAPI.Repositories
{
    public interface ITrackRepository : IDisposable
    {
        Task<List<Track>> GetTracksAsync();
        Task<List<Track>> GetPlaylistTracksAsync(Guid playlistId);
        Task<Track> GetTrackAsync(Guid trackId);
        Task InsertTrackAsync(Track track);
        Task<bool> UpdateTrackAsync(Track track);
        Task<bool> DeleteTrackAsync(Guid trackId);
        Task SaveAsync();
    }
}
