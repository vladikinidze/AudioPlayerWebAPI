namespace AudioPlayerWebAPI.Services.TokenService
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, User user);
        UserRefreshToken BuildRefreshToken(User user, string accessToken);
    }
}
