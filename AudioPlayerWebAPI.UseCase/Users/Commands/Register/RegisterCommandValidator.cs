using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(registerCommand => registerCommand.UserName)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(registerCommand => registerCommand.Email)
            .NotEmpty()
            .Matches(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}");
        RuleFor(registerCommand => registerCommand.Password)
            .NotEmpty()
            .MinimumLength(7);
        RuleFor(registerCommand  => registerCommand.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("'Confirm Password' must be equal to 'Password'.");
    }
}