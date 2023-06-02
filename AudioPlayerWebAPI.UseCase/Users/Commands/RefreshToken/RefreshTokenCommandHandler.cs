    using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.Services.TokenService;
using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthViewModel>
    {
        private readonly IAudioPlayerDbContext _context;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(IAudioPlayerDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AuthViewModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(request.AccessToken);
            var validTo = accessToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Expiration)!.Value;
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x =>
                    x.AccessToken == request.AccessToken &&
                    x.RefToken == request.RefreshToken &&
                    x.UserId == request.UserId, cancellationToken);

            if (refreshToken == null)
            {
                throw new RefreshTokenException("Invalid token.");
            }

            if (Convert.ToDateTime(validTo) > DateTime.Now)
            {
                throw new RefreshTokenException("Token not expired.");
            }

            if (!refreshToken.IsActive)
            {
                throw new RefreshTokenException("Refresh token expired.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
            return await _tokenService.BuildTokens(user!, cancellationToken);

        }
    }
}
