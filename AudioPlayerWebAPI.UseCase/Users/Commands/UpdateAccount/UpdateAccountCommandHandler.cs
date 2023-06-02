using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.Services.HashService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.UpdateAccount
{
    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, Unit>
    {
        private readonly IAudioPlayerDbContext _context;
        private readonly IHashService _hashService;

        public UpdateAccountCommandHandler(IAudioPlayerDbContext context, IHashService hashService)
        {
            _context = context;
            _hashService = hashService;
        }

        public async Task<Unit> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            //var password = _hashService.GetSha1Hash(request.Password);
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

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
