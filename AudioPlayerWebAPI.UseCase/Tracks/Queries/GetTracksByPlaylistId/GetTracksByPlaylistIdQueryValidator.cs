using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracksByPlaylistId
{
    public class GetTracksByPlaylistIdQueryValidator : AbstractValidator<GetTracksByPlaylistIdQuery>
    {
        public GetTracksByPlaylistIdQueryValidator()
        {
            RuleFor(tracksQuery => tracksQuery.PlaylistId).NotEqual(Guid.Empty);
            RuleFor(tracksQuery => tracksQuery.UserId).NotEqual(Guid.Empty);
        }
    }
}
