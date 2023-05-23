using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.DeletePlaylist
{
    public class DeletePlaylistCommandValidator : AbstractValidator<DeletePlaylistCommand>
    {
        public DeletePlaylistCommandValidator()
        {
            RuleFor(upc => upc.Id)
                .NotEqual(Guid.Empty);
            RuleFor(upc => upc.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
