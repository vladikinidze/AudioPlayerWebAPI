using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.CreatePlaylist
{
    public class CreatePlaylistCommandValidator : AbstractValidator<CreatePlaylistCommand>
    {
        public CreatePlaylistCommandValidator()
        {
            RuleFor(cpc => cpc.Title)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(cpc => cpc.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
