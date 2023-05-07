namespace AudioPlayerWebAPI.Repositories
{
    public interface IRefreshTokenRepository : IDisposable
    {
        Task<UserRefreshToken> GetAsync(string token, string refreshToken);
        Task SetRefreshTokenAsync(UserRefreshToken refreshToken);
        Task SaveAsync();
    }
}
