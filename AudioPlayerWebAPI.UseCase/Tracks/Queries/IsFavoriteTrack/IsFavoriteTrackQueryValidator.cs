using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Tracks.Queries.IsFavoriteTrack;

public class IsFavoriteTrackQueryValidator : AbstractValidator<IsFavoriteTrackQuery>
{
    public IsFavoriteTrackQueryValidator()
    {
        RuleFor(query => query.UserId).NotEqual(Guid.Empty);
        RuleFor(query => query.TrackId).NotEqual(Guid.Empty);
    }
}