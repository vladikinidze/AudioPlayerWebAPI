using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.IsAddedPlaylist;

public class IsAddedPlaylistQueryValidator : AbstractValidator<IsAddedPlaylistQuery>
{
    public IsAddedPlaylistQueryValidator()
    {
        RuleFor(x => x.UserId).NotEqual(Guid.Empty);
        RuleFor(x => x.PlaylistId).NotEqual(Guid.Empty);
    }
}