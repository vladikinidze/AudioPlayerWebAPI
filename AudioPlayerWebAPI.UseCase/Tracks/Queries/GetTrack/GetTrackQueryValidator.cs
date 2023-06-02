using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTrack
{
    public class GetTrackQueryValidator : AbstractValidator<GetTrackQuery>
    {
        public GetTrackQueryValidator()
        {
            RuleFor(trackQuery => trackQuery.Id).NotEqual(Guid.Empty);
        }
    }
}
