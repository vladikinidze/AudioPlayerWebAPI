using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.CreatePlaylist
{
    public class CreatePlaylistCommandValidator : AbstractValidator<CreatePlaylistCommand>
    {
        public CreatePlaylistCommandValidator()
        {
            RuleFor(playlistCommand => playlistCommand.Title)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(playlistCommand => playlistCommand.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
