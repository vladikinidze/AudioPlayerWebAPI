using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Commands.UpdatePlaylist
{
    public class UpdatePlaylistCommandValidator : AbstractValidator<UpdatePlaylistCommand>
    {
        public UpdatePlaylistCommandValidator()
        {
            RuleFor(upc => upc.Title)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(upc => upc.Id)
                .NotEqual(Guid.Empty);
            RuleFor(upc => upc.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
