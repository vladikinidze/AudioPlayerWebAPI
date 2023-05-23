using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(loginCommand => loginCommand.Email).NotEmpty().Matches(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}");
            RuleFor(loginCommand => loginCommand.Password).NotEmpty();
        }
    }
}
