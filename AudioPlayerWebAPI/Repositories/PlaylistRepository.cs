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
            await _context.Playlists
                .OrderByDescending(x => x.Created)
                .ToListAsync();

        public async Task<List<Playlist>> GetUserPlaylists(Guid userId)
        {
            var user = await _context.Users
                .Include(x => x.Playlists)
                .FirstOrDefaultAsync(x => x.Id == userId);
            return user!.Playlists;
        }

        public async Task<Playlist> GetPlaylistAsync(Guid playlistId) =>
            (await _context.Playlists
                .Include(x => x.Tracks)
                .FirstOrDefaultAsync(x => x.Id == playlistId))!;

        public async Task InsertPlaylistAsync(Playlist playlist) =>
            await _context.Playlists.AddAsync(playlist);

        public async Task<bool> UpdatePlaylistAsync(Playlist playlist)
        {
            var playlistFromDb = await _context.Playlists.FindAsync(playlist.Id);
            if (playlistFromDb == null) return false;
            playlistFromDb.Title = playlist.Title;
            playlistFromDb.Image = playlist.Image;
            playlistFromDb.Tracks = playlist.Tracks;
            return true;
        }

        public async Task<bool> DeletePlaylistAsync(Guid playlistId)
        {
            var playlistFromDb = await _context.Playlists.FirstOrDefaultAsync(x => x.Id == playlistId);
            if (playlistFromDb == null) return false;
            _context.Playlists.Remove(playlistFromDb);
            return true;
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
