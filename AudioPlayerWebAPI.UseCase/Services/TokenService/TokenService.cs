using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Dtos;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AudioPlayerWebAPI.UseCase.Services.TokenService;

public class TokenService : ITokenService
{
    private readonly IAudioPlayerDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly TimeSpan _expiryDuration = new(0, 1, 0);

    public TokenService(IAudioPlayerDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public string BuildToken(string key, string issuer, User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Expiration, DateTime.Now.Add(_expiryDuration).ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey,
            SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
            expires: DateTime.Now.Add(_expiryDuration), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public RefreshToken BuildRefreshToken(User user, string accessToken)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            AccessToken = accessToken,
            RefToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Created = DateTime.Now,
            Expiration = DateTime.Now.AddDays(15),
            UserId = user.Id,
            User = user
        };
    }

    public async Task<AuthViewModel> BuildTokens(User user, CancellationToken cancellationToken)
    {
        var accessToken = BuildToken(_configuration["Jwt:Key"]!,
            _configuration["Jwt:Issuer"]!, user);
        var refreshToken = BuildRefreshToken(user, accessToken);
        refreshToken = await SetRefreshTokenAsync(refreshToken);
        user.RefreshToken = refreshToken;
        await _context.SaveChangesAsync(cancellationToken);
        return new AuthViewModel
        {
            UserId = refreshToken.UserId,
            RefreshToken = refreshToken.RefToken,
            AccessToken = accessToken
        };
    }

    public async Task<RefreshToken> SetRefreshTokenAsync(RefreshToken refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.UserId == refreshToken.UserId);
        if (token != null)
        {
            _context.RefreshTokens.Remove(token);
        }

        await _context.RefreshTokens.AddAsync(refreshToken);
        return refreshToken;
    }

    public TokenDto ReadToken(string authorizationHeader)
    {
        var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(authorizationHeader.Split(' ').Last());
        var userId = accessToken.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = accessToken.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
        return new TokenDto { UserId = new Guid(userId), Email = email };
    }
}