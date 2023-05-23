using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracks
{
    public class GetTracksQueryValidator : AbstractValidator<GetTracksQuery>
    {
        public GetTracksQueryValidator()
        {
            RuleFor(gt => gt.PlaylistId).NotEqual(Guid.Empty);
            RuleFor(gt => gt.UserId).NotEqual(Guid.Empty);
        }
    }
}
