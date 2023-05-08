using AudioPlayerWebAPI.Models.DTO;
using AudioPlayerWebAPI.Repositories;
using FluentValidation;

namespace AudioPlayerWebAPI.Apis
{
    public class PlaylistApi : IApi
    {
        private readonly IPlaylistRepository _repository;
        private readonly IValidator<Playlist> _validator;

        public PlaylistApi(IPlaylistRepository repository, IValidator<Playlist> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public void Register(WebApplication application)
        {
            application.MapGet("/api/playlists", Get)
                .Produces<List<Playlist>>(StatusCodes.Status200OK);

            application.MapGet("/api/playlists/{id}", GetById)
                .Produces<Playlist>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            application.MapGet("/api/playlists/{userId}", GetByUserId)
                .Produces<Playlist>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            application.MapPost("/api/playlists", Post)
                .Accepts<Playlist>("application/json")
                .Produces<Playlist>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status403Forbidden);

            application.MapPut("/api/playlists", Put)
                .Accepts<Playlist>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status403Forbidden);

            application.MapDelete("/api/playlists/{id}", Delete)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status403Forbidden);
        }

        [AllowAnonymous]
        private async Task<IResult> Get(IPlaylistRepository repository) =>
                    Results.Ok(await _repository.GetPlaylistsAsync());

        [AllowAnonymous]
        private async Task<IResult> GetById(Guid playlistId) =>
                    await _repository.GetPlaylistAsync(playlistId) is Playlist playlist
                        ? Results.Ok(playlist)
                        : Results.NotFound();

        [Authorize]
        private async Task<IResult> GetByUserId(Guid userId)
        {
            var playlists = await _repository.GetUserPlaylists(userId);
            if (playlists == null)
            {
                return Results.NotFound("Not found");
            }
            return Results.Ok(playlists);
        }
            

        [Authorize]
        private async Task<IResult> Post([FromBody] Playlist playlist)
        {
            var validation = await _validator.ValidateAsync(playlist);
            if (!validation.IsValid)
            {
                return Results.ValidationProblem(validation.ToDictionary());
            }
            await _repository.InsertPlaylistAsync(playlist);
            await _repository.SaveAsync();
            return Results.Created($"$api/playlists/{playlist.Id}", playlist.Id);
        }

        [Authorize]
        private async Task<IResult> Put([FromBody] Playlist playlist, Guid userId)
        {
            var validation = await _validator.ValidateAsync(playlist);
            if (!validation.IsValid)
            {
                return Results.ValidationProblem(validation.ToDictionary());
            }
            if (playlist.ParentUserId != userId)
            {
                return Results.Forbid();
            }
            await _repository.UpdatePlaylistAsync(playlist);
            await _repository.SaveAsync();
            return Results.Ok();
        }

        [Authorize]
        private async Task<IResult> Delete(Guid playlistId, Guid userId)
        {
            var playlist = await _repository.GetPlaylistAsync(playlistId);
            if (playlist.ParentUserId != userId || playlist == null)
            {
                return Results.Forbid();
            }
            await _repository.DeletePlaylistAsync(playlistId);
            await _repository.SaveAsync();
            return Results.Ok();
        }
    }
}
