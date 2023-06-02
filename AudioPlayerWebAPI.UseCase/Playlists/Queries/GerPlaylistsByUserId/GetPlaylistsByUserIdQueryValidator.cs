using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GerPlaylistsByUserId
{
    public class GetPlaylistsByUserIdQueryValidator : AbstractValidator<GetPlaylistsByUserIdQuery>
    {
        public GetPlaylistsByUserIdQueryValidator()
        {
            RuleFor(query => query.UserId).NotEqual(Guid.Empty);
        }
    }
}
