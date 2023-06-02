using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylist
{
    public class GetPlaylistQueryValidator : AbstractValidator<GetPlaylistQuery>
    {
        public GetPlaylistQueryValidator()
        {
            RuleFor(playlistQuery => playlistQuery.Id).NotEqual(Guid.Empty);
        }
    }
}
