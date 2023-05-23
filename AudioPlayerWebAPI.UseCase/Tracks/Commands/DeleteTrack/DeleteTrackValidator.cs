using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteTrack
{
    public class DeleteTrackValidator : AbstractValidator<DeleteTrackCommand>
    {
        public DeleteTrackValidator()
        {
            RuleFor(dtc => dtc.PlaylistId).NotEqual(Guid.Empty);
            RuleFor(dtc => dtc.UserId).NotEqual(Guid.Empty);
            RuleFor(dtc => dtc.Id).NotEqual(Guid.Empty);
        }
    }
}
