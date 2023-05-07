using System.Globalization;

namespace AudioPlayerWebAPI.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly TimeSpan _expiryDuration = new(0, 30, 0);
        public string BuildToken(string key, string issuer, User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Expiration, DateTime.Now.Add(_expiryDuration).ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.Add(_expiryDuration), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            
        }

        public UserRefreshToken BuildRefreshToken(User user, string accessToken)
        {
            return new UserRefreshToken()
            {
                Id = Guid.NewGuid(),
                AccessToken = accessToken,
                RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.Now,
                Expiration = DateTime.Now.AddDays(15),
                UserId = user.Id,
            };
        }
    }
}
