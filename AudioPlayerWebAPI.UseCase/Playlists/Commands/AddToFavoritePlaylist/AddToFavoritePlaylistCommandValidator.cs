using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.AddToFavoritePlaylist;

public class AddToFavoritePlaylistCommandValidator : AbstractValidator<AddToFavoritePlaylistCommand>
{
    public AddToFavoritePlaylistCommandValidator()
    {
        RuleFor(x => x.TrackId).NotEqual(Guid.Empty);
        RuleFor(x => x.UserId).NotEqual(Guid.Empty);
    }
}