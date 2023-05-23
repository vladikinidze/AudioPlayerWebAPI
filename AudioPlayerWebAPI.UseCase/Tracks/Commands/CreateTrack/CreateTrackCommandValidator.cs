using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.CreateTrack
{
    public class CreateTrackCommandValidator : AbstractValidator<CreateTrackCommand>
    {
        public CreateTrackCommandValidator()
        {
            RuleFor(ctc => ctc.Title)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(ctc => ctc.UserId).NotEqual(Guid.Empty);
            RuleFor(ctc => ctc.PlaylistId).NotEqual(Guid.Empty);
        }
    }
}
