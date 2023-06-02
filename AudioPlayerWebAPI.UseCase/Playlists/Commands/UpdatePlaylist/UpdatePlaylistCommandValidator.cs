using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.UpdatePlaylist
{
    public class UpdatePlaylistCommandValidator : AbstractValidator<UpdatePlaylistCommand>
    {
        public UpdatePlaylistCommandValidator()
        {
            RuleFor(playlistCommand => playlistCommand.Title)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(playlistCommand => playlistCommand.Id)
                .NotEqual(Guid.Empty);
            RuleFor(playlistCommand => playlistCommand.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
