using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Errors;

public class ErrorCommandValidator : AbstractValidator<ErrorCommand>
{
    public ErrorCommandValidator()
    {
        RuleFor(x => x.Text).NotEmpty();
    }
}