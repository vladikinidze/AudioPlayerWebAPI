namespace AudioPlayerWebAPI.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AudioPlayerDbContext _context;

        public RefreshTokenRepository(AudioPlayerDbContext context)
        {
            _context = context;
        }

        public async Task<UserRefreshToken> GetAsync(string token, string refreshToken) =>
            (await _context.RefreshTokens.FirstOrDefaultAsync(t =>
                t.RefreshToken == refreshToken && t.AccessToken == token))!;


        public async Task<UserRefreshToken> SetRefreshTokenAsync(UserRefreshToken refreshToken)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == refreshToken.UserId);
            if (token != null)
            {
                _context.RefreshTokens.Remove(token);
            }
            return _context.RefreshTokens.AddAsync(refreshToken).Result.Entity;
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
