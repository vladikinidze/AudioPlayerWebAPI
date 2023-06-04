using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.Services.HashService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IAudioPlayerDbContext _context;
    private readonly IHashService _hashService;

    public RegisterCommandHandler(IAudioPlayerDbContext context, IHashService hashService)
    {
        _context = context;
        _hashService = hashService;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user != null)
        {
            throw new EmailAlreadyInUseException();
        }

        user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Username = request.UserName,
            Password = _hashService.GetSha1Hash(request.Password),
            Image = "548864f8-319e-40ac-9f9b-a31f65ccb902.jpg",
        };

        var playlist = new Playlist
        {
            Id = Guid.NewGuid(),
            Private = true,
            Image = "548864f8-319e-40ac-9f9b-a31f65ccb902.jpg",
            Title = "Favorite",
            CreationDate = DateTime.Now,
        };

        var userPlaylist = new UserPlaylists
        {
            Playlist = playlist,
            User = user,
            PlaylistId = playlist.Id,
            UserId = user.Id,
            AddedDate = DateTime.Now,
            IsOwner = true
        };

        var settings = new Settings
        {
            Explicit = false,
            User = user,
            Id = Guid.NewGuid()
        };
        
        await _context.Playlists.AddAsync(playlist, cancellationToken);
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.Settings.AddAsync(settings, cancellationToken);
        await _context.UserPlaylists.AddAsync(userPlaylist, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}