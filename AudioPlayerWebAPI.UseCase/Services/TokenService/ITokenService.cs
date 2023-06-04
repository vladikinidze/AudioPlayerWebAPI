using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Dtos;
using AudioPlayerWebAPI.UseCase.ViewModels;

namespace AudioPlayerWebAPI.UseCase.Services.TokenService;

public interface ITokenService
{
    string BuildToken(string key, string issuer, User user);
    RefreshToken BuildRefreshToken(User user, string accessToken);
    Task<AuthViewModel> BuildTokens(User user, CancellationToken cancellationToken);
    Task<RefreshToken> SetRefreshTokenAsync(RefreshToken refreshToken);
    TokenDto ReadToken(string authorizationHeader);
}