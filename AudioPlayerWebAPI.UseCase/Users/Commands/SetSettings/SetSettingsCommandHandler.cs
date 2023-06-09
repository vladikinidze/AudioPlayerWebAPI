﻿using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.SetSettings;

public class SetSettingsCommandHandler : IRequestHandler<SetSettingsCommand, Unit>
{
    private readonly IAudioPlayerDbContext _context;

    public SetSettingsCommandHandler(IAudioPlayerDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SetSettingsCommand request, CancellationToken cancellationToken)
    {
        var settings = await _context.Settings
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);

        if (settings == null)
        {
            throw new NotFoundException(nameof(User), request.UserId.ToString());
        }

        settings.Explicit = request.Explicit;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}