namespace AudioPlayerWebAPI.Repositories
{
    public interface IRefreshTokenRepository : IDisposable
    {
        Task<UserRefreshToken> GetAsync(string token, string refreshToken);
        Task<UserRefreshToken> SetRefreshTokenAsync(UserRefreshToken refreshToken);
        Task SaveAsync();
    }
}
