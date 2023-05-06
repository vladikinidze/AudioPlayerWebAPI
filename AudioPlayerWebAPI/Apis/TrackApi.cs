namespace AudioPlayerWebAPI.Apis
{
    public class TrackApi : IApi
    {
        public void Register(WebApplication application)
        {
            application.MapGet("/api/tracks", Get)
                .Produces<List<Track>>(StatusCodes.Status200OK);

            application.MapGet("/api/tracks/{id}", GetById)
                .Produces<Track>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            application.MapPost("/api/tracks", Post)
                .Accepts<Track>("application/json")
                .Produces<Track>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);
            

            application.MapPut("/api/tracks", Put)
                .Accepts<Track>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            application.MapDelete("/api/tracks/{id}", Delete)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
        }

        [AllowAnonymous]
        private async Task<IResult> Get(ITrackRepository repository) =>
            Results.Ok(await repository.GetTracksAsync());

        [AllowAnonymous]
        private async Task<IResult> GetById(Guid trackId, ITrackRepository repository) =>
            await repository.GetTrackAsync(trackId) is Track track
                ? Results.Ok(track)
                : Results.NotFound();

        [Authorize]
        private async Task<IResult> Post([FromBody] Track track, ITrackRepository repository)
        {
            await repository.InsertTrackAsync(track);
            await repository.SaveAsync();
            return Results.Created($"$tracks/{track.Id}", track.Id);
        }

        [Authorize]
        private async Task<IResult> Put([FromBody] Track track, ITrackRepository repository)
        {
            await repository.UpdateTrackAsync(track);
            await repository.SaveAsync();
            return Results.Ok();
        }

        [Authorize]
        private async Task<IResult> Delete(Guid trackId, ITrackRepository repository)
        {
            await repository.DeleteTrackAsync(trackId);
            await repository.SaveAsync();
            return Results.Ok();
        }
    }
}
