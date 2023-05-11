namespace AudioPlayerWebAPI.Repositories
{
    public interface IPlaylistRepository : IDisposable
    {
        Task<List<Playlist>> GetPlaylistsAsync();
        Task<List<Playlist>> GetUserPlaylists(Guid userId);
        Task<Playlist> GetPlaylistAsync(Guid playlistId);
        Task InsertPlaylistAsync(Playlist playlist);
        Task<bool> UpdatePlaylistAsync(Playlist playlist);
        Task<bool> DeletePlaylistAsync(Guid playlistId);
        Task SaveAsync();
    }
}
