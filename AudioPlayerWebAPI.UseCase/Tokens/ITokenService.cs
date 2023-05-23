﻿using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.ViewModels;

namespace AudioPlayerWebAPI.UseCase.Tokens
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, User user);
        RefreshToken BuildRefreshToken(User user, string accessToken);
        Task<AuthViewModel> BuildTokens(User user, CancellationToken cancellationToken);
        Task<RefreshToken> SetRefreshTokenAsync(RefreshToken refreshToken);
    }
}