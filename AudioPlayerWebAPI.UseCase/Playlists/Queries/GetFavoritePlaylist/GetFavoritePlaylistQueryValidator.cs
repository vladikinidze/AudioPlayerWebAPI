using AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylist;
using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Playlists.Queries.GetFavoritePlaylist;

public class GetFavoritePlaylistQueryValidator : AbstractValidator<GetFavoritePlaylistQuery>
{
    public GetFavoritePlaylistQueryValidator()
    {
        RuleFor(playlistQuery => playlistQuery.UserId).NotEqual(Guid.Empty);
    }
}