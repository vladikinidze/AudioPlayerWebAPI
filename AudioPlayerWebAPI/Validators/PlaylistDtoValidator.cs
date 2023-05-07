namespace AudioPlayerWebAPI.Validators
{
    public class PlaylistDtoValidator : AbstractValidator<Playlist>
    {
        public PlaylistDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.");
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
