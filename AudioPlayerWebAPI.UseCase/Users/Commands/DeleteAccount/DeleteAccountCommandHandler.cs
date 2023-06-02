using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.Services.HashService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.DeleteAccount
{
    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Unit>
    {
        private readonly IAudioPlayerDbContext _context;
        private readonly IHashService _hashService;

        public DeleteAccountCommandHandler(IAudioPlayerDbContext context, IHashService hashService)
        {
            _context = context;
            _hashService = hashService;
        }

        public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var password = _hashService.GetSha1Hash(request.Password);
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.UserId && x.Password == password, cancellationToken);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

            throw new NotFoundException(nameof(User), request.UserId.ToString());
        }
    }
}
