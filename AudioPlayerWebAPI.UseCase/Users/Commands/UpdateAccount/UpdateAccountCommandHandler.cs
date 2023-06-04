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
            user.Username = request.Username;
            
            if (request.Image != null)
            {
                if (user.Image != null && user.Image != "548864f8-319e-40ac-9f9b-a31f65ccb902.jpg")
                {
                    var image = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files\\Image", user.Image);
                    if (File.Exists(image))
                    {
                        File.Delete(image);
                    }
                }
                user.Image = request.Image;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
