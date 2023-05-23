using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTrack
{
    public class GetTrackQueryValidator : AbstractValidator<GetTrackQuery>
    {
        public GetTrackQueryValidator()
        {
            RuleFor(gt => gt.Id).NotEqual(Guid.Empty);
        }
    }
}
