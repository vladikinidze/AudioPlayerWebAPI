using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.Tokens;
using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthViewModel>
    {
        private readonly IAudioPlayerDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IAudioPlayerDbContext context, IConfiguration configuration, ITokenService tokenService)
        {
            _context = context;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<AuthViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email
                                          && u.Password == request.Password, cancellationToken);

            return user == null
                ? throw new NotFoundException(nameof(User), request.Email)
                : await _tokenService.BuildTokens(user, cancellationToken);
        }
    }
}
