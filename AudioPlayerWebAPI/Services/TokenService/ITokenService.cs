namespace AudioPlayerWebAPI.Services.TokenService
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, User user);
    }
}
