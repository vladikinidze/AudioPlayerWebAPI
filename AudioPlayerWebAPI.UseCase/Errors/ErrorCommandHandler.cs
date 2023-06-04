using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Errors;

public class ErrorCommandHandler : IRequestHandler<ErrorCommand, Unit>
{
    private readonly IAudioPlayerDbContext _context;

    public ErrorCommandHandler(IAudioPlayerDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(ErrorCommand request, CancellationToken cancellationToken)
    {
        var error = new Error
        {
            Id = Guid.NewGuid(),
            Text = request.Text,
        };

        await _context.Errors.AddAsync(error, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}