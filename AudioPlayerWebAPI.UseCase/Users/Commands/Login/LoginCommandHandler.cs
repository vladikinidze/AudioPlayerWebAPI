using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.Services.HashService;
using AudioPlayerWebAPI.UseCase.Services.TokenService;
using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthViewModel>
{
    private readonly IAudioPlayerDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IHashService _hashService;

    public LoginCommandHandler(IAudioPlayerDbContext context, 
        ITokenService tokenService, IHashService hashService)
    {
        _context = context;
        _tokenService = tokenService;
        _hashService = hashService;
    }

    public async Task<AuthViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var password = _hashService.GetSha1Hash(request.Password);
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email
                                      && u.Password == password, cancellationToken);

        return user == null
            ? throw new InvalidLoginException("Invalid Login or password")
            : await _tokenService.BuildTokens(user, cancellationToken);
    }
}