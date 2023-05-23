using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.DeletePlaylist
{
    public class DeletePlaylistCommandValidator : AbstractValidator<DeletePlaylistCommand>
    {
        public DeletePlaylistCommandValidator()
        {
            RuleFor(playlistCommand => playlistCommand.Id)
                .NotEqual(Guid.Empty);
            RuleFor(playlistCommand => playlistCommand.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
