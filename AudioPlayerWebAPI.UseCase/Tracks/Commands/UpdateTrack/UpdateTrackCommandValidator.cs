using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.UpdateTrack
{
    public class UpdateTrackCommandValidator : AbstractValidator<UpdateTrackCommand>
    {
        public UpdateTrackCommandValidator()
        {
            RuleFor(trackCommand => trackCommand.Title).NotEmpty().MaximumLength(100);
            RuleFor(trackCommand => trackCommand.PlaylistId).NotEqual(Guid.Empty);
            RuleFor(trackCommand => trackCommand.UserId).NotEqual(Guid.Empty);
            RuleFor(trackCommand => trackCommand.Id).NotEqual(Guid.Empty);
        }
    }
}
