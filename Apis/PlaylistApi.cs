namespace AudioPlayerWebAPI.Apis
{
    public class PlaylistApi : IApi
    {
        public void Register(WebApplication application)
        {
            application.MapGet("/api/playlists", Get)
                .Produces<List<Playlist>>(StatusCodes.Status200OK);

            application.MapGet("/api/playlists/{id}", GetById)
                .Produces<Playlist>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            //application.MapGet("/playlists/{userId}", GetByUserId)
            //    .Produces<Playlist>(StatusCodes.Status200OK)
            //    .Produces(StatusCodes.Status400BadRequest);

            application.MapPost("/api/playlists", Post)
                .Accepts<Playlist>("application/json")
                .Produces<Playlist>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            application.MapPut("/api/playlists", Put)
                .Accepts<Playlist>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            application.MapDelete("/api/playlists/{id}", Delete)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
        }

        [AllowAnonymous]
        private async Task<IResult> Get(IPlaylistRepository repository) =>
                    Results.Ok(await repository.GetPlaylistsAsync());

        [AllowAnonymous]
        private async Task<IResult> GetById(Guid playlistId, IPlaylistRepository repository) =>
                    await repository.GetPlaylistAsync(playlistId) is Playlist playlist
                        ? Results.Ok(playlist)
                        : Results.NotFound();

        //[Authorize]
        //private async Task<IResult> GetByUserId(Guid userId, IPlaylistRepository repository) =>
        //    Results.Ok(await repository.GetPlaylistAsync(userId));

        [Authorize]
        private async Task<IResult> Post([FromBody] Playlist playlist, IPlaylistRepository repository)
        {
            await repository.InsertPlaylistAsync(playlist);
            await repository.SaveAsync();
            return Results.Created($"$Playlists/{playlist.Id}", playlist.Id);
        }

        [Authorize]
        private async Task<IResult> Put([FromBody] Playlist Playlist, IPlaylistRepository repository)
        {
            await repository.UpdatePlaylistAsync(Playlist);
            await repository.SaveAsync();
            return Results.Ok();
        }

        [Authorize]
        private async Task<IResult> Delete(Guid playlistId, IPlaylistRepository repository)
        {
            await repository.DeletePlaylistAsync(playlistId);
            await repository.SaveAsync();
            return Results.Ok();
        }
    }
}
