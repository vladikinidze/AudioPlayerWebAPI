namespace AudioPlayerWebAPI.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        private readonly AudioPlayerDbContext _context;

        public TrackRepository(AudioPlayerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Track>> GetTracksAsync() =>
            await _context.Tracks.ToListAsync();

        public async Task<List<Track>> GetPlaylistTracksAsync(Guid playlistId) => 
            ((await _context.Playlists.FirstOrDefaultAsync(x => x.Id == playlistId))!).Tracks.ToList();
        
        public async Task<Track> GetTrackAsync(Guid trackId) =>
            (await _context.Tracks.FindAsync(trackId))!;

        public async Task InsertTrackAsync(Track track) =>
            await _context.Tracks.AddAsync(track);

        public async Task UpdateTrackAsync(Track track)
        {
            var trackFromDb = await _context.Tracks.FindAsync(track.Id);
            if (trackFromDb == null) return;
            trackFromDb.Title = track.Title;
            trackFromDb.Text = track.Text;
            trackFromDb.Audio = track.Audio;
            trackFromDb.Explicit = track.Explicit;
            trackFromDb.Playlists = track.Playlists;
        }

        public async Task DeleteTrackAsync(Guid trackId)
        {
            var trackFromDb = await _context.Tracks.FindAsync(trackId);
            if (trackFromDb == null) return;
            _context.Tracks.Remove(trackFromDb);
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
