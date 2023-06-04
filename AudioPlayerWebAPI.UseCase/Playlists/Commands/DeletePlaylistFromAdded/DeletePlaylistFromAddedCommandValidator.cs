using AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteFromFavoritePlaylist;
using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.DeletePlaylistFromAdded;

public class DeletePlaylistFromAddedCommandValidator : AbstractValidator<DeleteFromFavoritePlaylistCommand>
{
public DeletePlaylistFromAddedCommandValidator()
{
    RuleFor(x => x.TrackId).NotEqual(Guid.Empty);
    RuleFor(x => x.UserId).NotEqual(Guid.Empty);
}    
}