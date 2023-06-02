using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.DeleteAccount
{
    public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
    {
        public DeleteAccountCommandValidator()
        {
            RuleFor(dc => dc.UserId).NotEqual(Guid.Empty);
        }
    }
}
