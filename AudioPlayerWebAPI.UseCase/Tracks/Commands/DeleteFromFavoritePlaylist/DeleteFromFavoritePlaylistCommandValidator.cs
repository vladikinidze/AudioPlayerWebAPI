using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteFromFavoritePlaylist;

public class DeleteFromFavoritePlaylistCommandValidator: AbstractValidator<DeleteFromFavoritePlaylistCommand>
{
    public DeleteFromFavoritePlaylistCommandValidator()
    {
        RuleFor(x => x.TrackId).NotEqual(Guid.Empty);
        RuleFor(x => x.UserId).NotEqual(Guid.Empty);
    }
}