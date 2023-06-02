using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(tokenCommand => tokenCommand.UserId).NotEqual(Guid.Empty);
            RuleFor(tokenCommand => tokenCommand.AccessToken).NotEmpty();
            RuleFor(tokenCommand => tokenCommand.RefreshToken).NotEmpty();
        }
    }
}
