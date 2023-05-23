using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.UpdateAccount
{
    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, Unit>
    {
        private readonly IAudioPlayerDbContext _context;

        public UpdateAccountCommandHandler(IAudioPlayerDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email 
                                          && x.Password == request.Password, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.Id.ToString());
            }
            user.Email = request.Email;
            user.Image = request.Image;
            user.Username = request.Username;
            return Unit.Value;
        }
    }
}
