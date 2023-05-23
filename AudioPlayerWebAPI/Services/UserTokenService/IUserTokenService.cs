namespace AudioPlayerWebAPI.Services.UserTokenService
{
    public interface IUserTokenService
    {
        Guid GetUserId(string authorizationHeader);
    }
}
