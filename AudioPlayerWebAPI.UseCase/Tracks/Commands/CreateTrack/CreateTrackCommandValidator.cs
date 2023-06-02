using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.CreateTrack
{
    public class CreateTrackCommandValidator : AbstractValidator<CreateTrackCommand>
    {
        public CreateTrackCommandValidator()
        {
            RuleFor(trackCommand => trackCommand.Title)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(trackCommand => trackCommand.UserId).NotEqual(Guid.Empty);
            RuleFor(trackCommand => trackCommand.PlaylistId).NotEqual(Guid.Empty);
            RuleFor(trackCommand => trackCommand.Audio).NotEmpty();
        }
    }
}
