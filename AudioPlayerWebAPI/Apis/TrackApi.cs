﻿using AudioPlayerWebAPI.Models;
using AudioPlayerWebAPI.Models.DTO;
using AudioPlayerWebAPI.Repositories;
using FluentValidation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace AudioPlayerWebAPI.Apis
{
    public class TrackApi : IApi
    {
        private readonly ITrackRepository _trackRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IValidator<TrackDto> _validator;
        private readonly IMapper _mapper;

        public TrackApi(ITrackRepository trackRepository, IPlaylistRepository playlistRepository,
            IValidator<TrackDto> validator, IMapper mapper)
        {
            _trackRepository = trackRepository;
            _playlistRepository = playlistRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public void Register(WebApplication application)
        {
            application.MapGet("/api/tracks", Get)
                .Produces<List<TrackDto>>(StatusCodes.Status200OK);

            application.MapGet("/api/tracks/{trackId}", GetById)
                .Produces<TrackDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
            application.MapGet("/api/tracks/playlists/{playlistId}", GetByPlaylistId)
                .Produces<List<TrackDto>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            application.MapPost("/api/tracks", Post)
                .Accepts<TrackDto>("application/json")
                .Produces<TrackDto>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);
            
            application.MapPut("/api/tracks", Put)
                .Accepts<Track>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            application.MapDelete("/api/tracks/{trackId}", Delete)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
        }

        [AllowAnonymous]
        private async Task<IResult> Get() =>
            Results.Ok(_mapper.Map<List<TrackDto>>(await _trackRepository.GetTracksAsync()));

        [AllowAnonymous]
        private async Task<IResult> GetById(Guid trackId) =>
            await _trackRepository.GetTrackAsync(trackId) is Track track
                ? Results.Ok(_mapper.Map<TrackDto>(track))
                : Results.NotFound();

        [AllowAnonymous]
        private async Task<IResult> GetByPlaylistId(Guid playlistId)
        {
            var tracks = _mapper.Map<List<TrackDto>>(await _trackRepository.GetPlaylistTracksAsync(playlistId));
            return Results.Ok(tracks);
        }
         

        [Authorize]
        private async Task<IResult> Post([FromBody] TrackDto trackDto)
        {
            var validation = await _validator.ValidateAsync(trackDto);
            if (!validation.IsValid)
            {
                return Results.ValidationProblem(validation.ToDictionary());
            }
            var track = _mapper.Map<Track>(trackDto);
            var playlist = await _playlistRepository.GetPlaylistAsync(track.ParentPlaylistId);
            if (playlist == null)
            {
                return Results.BadRequest("Playlist not found");
            }
            track.Id = Guid.NewGuid();
            track.Playlists.Add(playlist);
            playlist.Tracks.Add(track);
            await _trackRepository.InsertTrackAsync(track);
            await _trackRepository.SaveAsync();
            return Results.Created($"$api/tracks/{track.Id}", track.Id);
        }

        [Authorize]
        private async Task<IResult> Put([FromBody] TrackDto trackDto)
        {
            var validation = await _validator.ValidateAsync(trackDto);
            if (!validation.IsValid)
            {
                return Results.ValidationProblem(validation.ToDictionary());
            }
            var isSuccess = await _trackRepository.UpdateTrackAsync(_mapper.Map<Track>(trackDto));
            if (!isSuccess)
            {
                return Results.NotFound();
            }
            await _trackRepository.SaveAsync();
            return Results.Ok();
        }

        [Authorize]
        private async Task<IResult> Delete(Guid trackId, Guid playlistId)
        {
            var track = await _trackRepository.GetTrackAsync(trackId);
            if (track.ParentPlaylistId != playlistId)
            {
                Results.Forbid();
            }
            var isSuccess = await _trackRepository.DeleteTrackAsync(trackId);
            if (!isSuccess)
            {
                return Results.NotFound();
            }
            await _trackRepository.SaveAsync();
            return Results.Ok();
        }
    }
}
