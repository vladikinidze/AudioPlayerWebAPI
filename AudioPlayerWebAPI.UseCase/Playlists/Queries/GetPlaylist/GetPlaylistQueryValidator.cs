using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylist
{
    public class GetPlaylistQueryValidator : AbstractValidator<GetPlaylistQuery>
    {
        public GetPlaylistQueryValidator()
        {
            RuleFor(gpq => gpq.Id).NotEqual(Guid.Empty);
        }
    }
}
