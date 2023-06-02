using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.AddPlaylist;

public class AddPlaylistCommandValidator : AbstractValidator<AddPlaylistCommand>
{
    public AddPlaylistCommandValidator()
    {
        RuleFor(x => x.PlaylistId).NotEqual(Guid.Empty);
        RuleFor(x => x.UserId).NotEqual(Guid.Empty);
    }    
}