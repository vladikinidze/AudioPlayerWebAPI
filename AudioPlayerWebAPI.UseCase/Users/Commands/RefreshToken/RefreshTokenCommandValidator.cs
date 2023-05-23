using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand.RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(tokenCommand => tokenCommand.AccessToken).NotEmpty();
            RuleFor(tokenCommand => tokenCommand.RefreshToken).NotEmpty();
        }
    }
}
