namespace AudioPlayerWebAPI.Validators
{
    public class TrackDtoValidator : AbstractValidator<TrackDto>
    {
        public TrackDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.");
            RuleFor(x => x.Audio)
                .NotEmpty()
                .WithMessage("Audio is required.");
        }
    }
}
