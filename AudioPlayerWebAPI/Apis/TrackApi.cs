using AudioPlayerWebAPI.Models;
using AudioPlayerWebAPI.Repositories;

namespace AudioPlayerWebAPI.Apis
{
    public class TrackApi : IApi
    {
        private readonly ITrackRepository _repository;

        public TrackApi(ITrackRepository repository)
        {
            _repository = repository;
        }

        public void Register(WebApplication application)
        {
            application.MapGet("/api/tracks", Get)
                .Produces<List<Track>>(StatusCodes.Status200OK);

            application.MapGet("/api/tracks/{playlistId}", GetTracksByPlaylistId)
                .Produces<List<Track>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

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
        private async Task<IResult> Get() =>
            Results.Ok(await _repository.GetTracksAsync());

        [AllowAnonymous]
        private async Task<IResult> GetTracksByPlaylistId(Guid playlistId) =>
            Results.Ok(await _repository.GetPlaylistTracksAsync(playlistId));

        [AllowAnonymous]
        private async Task<IResult> GetById(Guid trackId) =>
            await _repository.GetTrackAsync(trackId) is Track track
                ? Results.Ok(track)
                : Results.NotFound();

        [Authorize]
        private async Task<IResult> Post([FromBody] Track track)
        {
            await _repository.InsertTrackAsync(track);
            await _repository.SaveAsync();
            return Results.Created($"$tracks/{track.Id}", track.Id);
        }

        [Authorize]
        private async Task<IResult> Put([FromBody] Track track)
        {
            await _repository.UpdateTrackAsync(track);
            await _repository.SaveAsync();
            return Results.Ok();
        }

        [Authorize]
        private async Task<IResult> Delete(Guid trackId)
        {
            await _repository.DeleteTrackAsync(trackId);
            await _repository.SaveAsync();
            return Results.Ok();
        }
    }
}
