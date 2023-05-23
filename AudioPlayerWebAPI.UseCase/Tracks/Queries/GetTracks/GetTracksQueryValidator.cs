using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracks
{
    public class GetTracksQueryValidator : AbstractValidator<GetTracksQuery>
    {
        public GetTracksQueryValidator()
        {
            RuleFor(tracksQuery => tracksQuery.PlaylistId).NotEqual(Guid.Empty);
            RuleFor(tracksQuery => tracksQuery.UserId).NotEqual(Guid.Empty);
        }
    }
}
