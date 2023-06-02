using Azure.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AudioPlayerWebAPI.Services.UserTokenService
{
    public class UserTokenService : IUserTokenService
    {
        public Guid GetUserId(string authorizationHeader)
        {
            var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(authorizationHeader.Split(' ').Last());
            var userId = accessToken.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            return new Guid(userId);
        }
    }
}
