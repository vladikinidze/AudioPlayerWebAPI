namespace AudioPlayerWebAPI.Repositories
{
    public interface IPlaylistRepository : IDisposable
    {
        Task<List<Playlist>> GetPlaylistsAsync();
        Task<List<Playlist>> GetUserPlaylists(Guid userId);
        Task<Playlist> GetPlaylistAsync(Guid playlistId);
        Task InsertPlaylistAsync(Playlist playlist);
        Task UpdatePlaylistAsync(Playlist playlist);
        Task DeletePlaylistAsync(Guid playlistId);
        Task SaveAsync();
    }
}
