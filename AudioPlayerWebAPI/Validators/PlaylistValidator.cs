namespace AudioPlayerWebAPI.Validators
{
    public class PlaylistValidator : AbstractValidator<Playlist>
    {
        public PlaylistValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.");
            RuleFor(x => x.ParentUserId)
                .NotEmpty();
        }
    }
}
