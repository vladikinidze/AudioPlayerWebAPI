using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteTrack
{
    public class DeleteTrackValidator : AbstractValidator<DeleteTrackCommand>
    {
        public DeleteTrackValidator()
        {
            RuleFor(trackCommand => trackCommand.PlaylistId).NotEqual(Guid.Empty);
            RuleFor(trackCommand => trackCommand.UserId).NotEqual(Guid.Empty);
            RuleFor(trackCommand => trackCommand.Id).NotEqual(Guid.Empty);
        }
    }
}
