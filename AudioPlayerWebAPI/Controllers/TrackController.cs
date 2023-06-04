using AudioPlayerWebAPI.Models;
using AudioPlayerWebAPI.Services.FileService;
using AudioPlayerWebAPI.Types;
using AudioPlayerWebAPI.UseCase.Services.TokenService;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.AddToFavoritePlaylist;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.CreateTrack;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteFromFavoritePlaylist;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteTrack;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.UpdateTrack;
using AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTrack;
using AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracks;
using AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracksByPlaylistId;
using AudioPlayerWebAPI.UseCase.Tracks.Queries.IsFavoriteTrack;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPlayerWebAPI.Controllers;

[ApiVersionNeutral]
[ApiController]
[Route("api/{version:apiVersion}/[controller]")]
public class TrackController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IFileService _fileService;
    private readonly ITokenService _tokenService;

    public TrackController(IMapper mapper, IMediator mediator,
        IFileService fileService, ITokenService tokenService)
    {
        _mapper = mapper;
        _mediator = mediator;
        _fileService = fileService;
        _tokenService = tokenService;
    }
    
    /// <summary>
    /// Gets the track list
    /// </summary>
    /// <returns>Return TrackListVm</returns>
    /// <response code="200">Success</response>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<TrackListViewModel>> GetTrack()
    {
        var query = new GetTracksQuery();
        if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Authorization"].ToString()))
        {
            var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
            query.UserId = userId;
        }
        var vm = await _mediator.Send(query);
        return Ok(vm);
    }
    
    /// <summary>
    /// Gets the track by Id
    /// </summary>
    /// <param name="getTrackDto">GetTrackDto object</param>
    /// <returns>Return TrackListVm</returns>
    /// <response code="200">Success</response>
    /// <response code="404">Not found</response>
    [HttpGet("Playlist")]
    [AllowAnonymous]
    public async Task<ActionResult<TrackListViewModel>> GetTrack([FromBody] GetTrackDto getTrackDto)
    {
        var query = _mapper.Map<GetTrackQuery>(getTrackDto);
        if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Authorization"].ToString()))
        {
            var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
            query.UserId = userId;
        }
        var vm = await _mediator.Send(query);
        return Ok(vm);
    }

    /// <summary>
    /// Gets the list of tracks by Playlist Id
    /// </summary>
    /// <param name="playlistId">Playlist Id (guid)</param>
    /// <returns>Return TrackListVm</returns>
    /// <response code="200">Success</response>
    /// <response code="404">Not found</response>
    [HttpGet("{playlistId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<TrackListViewModel>> GetByPlaylistId(Guid playlistId)
    {
        var query = new GetTracksByPlaylistIdQuery {PlaylistId = playlistId};
        if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Authorization"].ToString()))
        {
            var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
            query.UserId = userId;
        }
        var vm = await _mediator.Send(query);
        return Ok(vm);
    }

    /// <summary>
    /// Create the track
    /// </summary>
    /// <param name="createTrackDto">CreateTrackDto object</param>
    /// <returns>Return Id (guid)</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Guid>> Post([FromForm] CreateTrackDto createTrackDto)
    {
        var audioPath = await _fileService.Upload(createTrackDto.Audio, FileType.Audio);
        var command = _mapper.Map<CreateTrackCommand>(createTrackDto);
        if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Authorization"].ToString()))
        {
            var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
            command.UserId = userId;
        }
        command.Audio = audioPath;
        var playlistId = await _mediator.Send(command);
        return Ok(playlistId);
    }

    /// <summary>
    /// Update the track
    /// </summary>
    /// <param name="updateTrackDto">UpdateTrackDto object</param>
    /// <response code="200">Success</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Not found</response>
    /// <response code="401">Unauthorized</response>
    [HttpPut]
    [Authorize]
    public async Task<ActionResult<Guid>> Update([FromForm] UpdateTrackDto updateTrackDto)
    {
        var command = _mapper.Map<UpdateTrackCommand>(updateTrackDto);
        if (updateTrackDto.Audio != null)
        {
            var audioPath = await _fileService.Upload(updateTrackDto.Audio, FileType.Audio);
            command.Audio = audioPath;
        }
        var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
        command.UserId = userId;
        await _mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Delete the track
    /// </summary>
    /// <param name="deleteTrackDto">DeleteTrackDto object</param>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not found</response>
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete(DeleteTrackDto deleteTrackDto)
    {
        var command = new DeleteTrackCommand
        {
            Id = deleteTrackDto.Id, 
            PlaylistId = deleteTrackDto.PlaylistId
        };
        if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Authorization"].ToString()))
        {
            var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
            command.UserId = userId;
        }
        await _mediator.Send(command);
        return Ok();
    }
    
    /// <summary>
    /// Add the track to favorite playlist 
    /// </summary>
    /// <param name="trackId">track id</param>
    /// <response code="200">Success</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost("User/AddToFavorite/{trackId:guid}")]
    [Authorize]
    public async Task<IActionResult> AddToFavorite(Guid trackId)
    {
        var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
        var command = new AddToFavoritePlaylistCommand
        {
            UserId = userId,
            TrackId = trackId
        };
        await _mediator.Send(command);
        return Ok();
    }
    
    /// <summary>
    /// Delete the track to favorite playlist 
    /// </summary>
    /// <param name="trackId">track id</param>
    /// <response code="200">Success</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized</response>
    [HttpDelete("User/DeleteFromFavorite/{trackId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteFromFavorite(Guid trackId)
    {
        var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
        var command = new DeleteFromFavoritePlaylistCommand()
        {
            UserId = userId,
            TrackId = trackId
        };
        await _mediator.Send(command);
        return Ok();
    }
    
    /// <summary>
    /// The track has been added by the favorite user track list
    /// </summary>
    /// <param name="trackId">Track Id (guid)</param>
    /// <returns>Return bool</returns>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">NotFound</response>
    [HttpGet("User/IsFavorite/{trackId:guid}")]
    [Authorize]
    public async Task<ActionResult<bool>> IsFavorite(Guid trackId)
    {
        var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
        var query = new IsFavoriteTrackQuery() { UserId = userId, TrackId = trackId};
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}