using AudioPlayerWebAPI.Models;
using AudioPlayerWebAPI.Models.DTO;
using AudioPlayerWebAPI.Repositories;
using FluentValidation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace AudioPlayerWebAPI.Apis
{
    public class TrackApi : IApi
    {
        private readonly ITrackRepository _repository;
        private readonly IValidator<TrackDto> _validator;
        private readonly IMapper _mapper;

        public TrackApi(ITrackRepository repository, IValidator<TrackDto> validator, IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

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
        private async Task<IResult> Get() =>
            Results.Ok(await _repository.GetTracksAsync());

        [AllowAnonymous]
        private async Task<IResult> GetById(Guid trackId) =>
            await _repository.GetTrackAsync(trackId) is Track track
                ? Results.Ok(track)
                : Results.NotFound();

        [Authorize]
        private async Task<IResult> Post([FromBody] TrackDto trackDto)
        {
            var validation = await _validator.ValidateAsync(trackDto);
            if (!validation.IsValid)
            {
                return Results.ValidationProblem(validation.ToDictionary());
            }
            await _repository.InsertTrackAsync(_mapper.Map<Track>(trackDto));
            await _repository.SaveAsync();
            return Results.Created($"$api/tracks/{trackDto.Id}", trackDto.Id);
        }

        [Authorize]
        private async Task<IResult> Put([FromBody] TrackDto trackDto)
        {
            var validation = await _validator.ValidateAsync(trackDto);
            if (!validation.IsValid)
            {
                return Results.ValidationProblem(validation.ToDictionary());
            }
            await _repository.UpdateTrackAsync(_mapper.Map<Track>(trackDto));
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
