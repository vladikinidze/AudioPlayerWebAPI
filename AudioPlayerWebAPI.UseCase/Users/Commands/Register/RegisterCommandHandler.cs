using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
    {
        private readonly IAudioPlayerDbContext _context;

        public RegisterCommandHandler(IAudioPlayerDbContext context)
        {
            _context = context;
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
                Password = request.Password,
                Image = "548864f8-319e-40ac-9f9b-a31f65ccb902.png",
            };

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return user.Id;
        }
    }
}
