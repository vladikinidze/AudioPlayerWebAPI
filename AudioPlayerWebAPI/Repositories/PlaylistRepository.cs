namespace AudioPlayerWebAPI.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly AudioPlayerDbContext _context;

        public PlaylistRepository(AudioPlayerDbContext context)
        {
            _context = context;
        }
        public async Task<List<Playlist>> GetPlaylistsAsync() =>
            await _context.Playlists.ToListAsync();

        public async Task<List<Playlist>> GetUserPlaylists(Guid userId) => 
            await _context.Playlists.Where(x => x.UserId == userId).ToListAsync();

        public async Task<Playlist> GetPlaylistAsync(Guid playlistId) =>
            (await _context.Playlists.FindAsync(playlistId))!;

        public async Task InsertPlaylistAsync(Playlist playlist) =>
            await _context.Playlists.AddAsync(playlist);

        public async Task UpdatePlaylistAsync(Playlist playlist)
        {
            var playlistFromDb = await _context.Playlists.FindAsync(playlist.Id);
            if (playlistFromDb == null) return;
            playlistFromDb.Title = playlist.Title;
            playlistFromDb.Image = playlist.Image;
            playlistFromDb.Tracks = playlist.Tracks;
        }

        public async Task DeletePlaylistAsync(Guid playlistId)
        {
            var playlistFromDb = await _context.Playlists.FindAsync(playlistId);
            if (playlistFromDb == null) return;
            _context.Playlists.Remove(playlistFromDb);
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
