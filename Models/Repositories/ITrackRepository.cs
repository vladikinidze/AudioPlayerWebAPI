namespace AudioPlayerWebAPI.Models.Repositories
{
    public interface ITrackRepository : IDisposable
    {
        Task<List<Track>> GetTracksAsync();
        Task<Track> GetTrackAsync(Guid trackId);
        Task InsertTrackAsync(Track track);
        Task UpdateTrackAsync(Track track);
        Task DeleteTrackAsync(Guid trackId);
        Task SaveAsync();
    }
}
