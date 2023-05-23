using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.UpdateTrack
{
    public class UpdateTrackCommandValidator : AbstractValidator<UpdateTrackCommand>
    {
        public UpdateTrackCommandValidator()
        {
            RuleFor(utc => utc.Title).NotEmpty().MaximumLength(100);
            RuleFor(utc => utc.PlaylistId).NotEqual(Guid.Empty);
            RuleFor(utc => utc.UserId).NotEqual(Guid.Empty);
            RuleFor(utc => utc.Id).NotEqual(Guid.Empty);
        }
    }
}
