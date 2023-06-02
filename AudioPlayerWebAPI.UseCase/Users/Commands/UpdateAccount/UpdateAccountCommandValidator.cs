using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.UpdateAccount
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator()
        {
            RuleFor(updateCommand => updateCommand.Id)
                .NotEqual(Guid.Empty);
            RuleFor(updateCommand => updateCommand.Username)
                .NotEmpty()
                .MaximumLength(50);
            RuleFor(updateCommand => updateCommand.Email)
                .NotEmpty()
                .Matches(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}");
        }
    }
}
