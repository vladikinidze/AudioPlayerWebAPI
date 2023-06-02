using AudioPlayerWebAPI.Models;
using AudioPlayerWebAPI.Services;
using AudioPlayerWebAPI.Services.FileService;
using AudioPlayerWebAPI.Services.UserTokenService;
using AudioPlayerWebAPI.Types;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.CreateTrack;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteTrack;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.UpdateTrack;
using AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracksByPlaylistId;
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
    private readonly IUserTokenService _tokenService;

    public TrackController(IMapper mapper, IMediator mediator, IFileService fileService, IUserTokenService tokenService)
    {
        _mapper = mapper;
        _mediator = mediator;
        _fileService = fileService;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Gets the list of tracks by Playlist Id
    /// </summary>
    /// <param name="playlistId">Playlist Id (guid)</param>
    /// <returns>Return TrackListVm</returns>
    /// <response code="200">Success</response>
    /// <response code="404">Not found</response>
    [HttpGet("{playlistId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TrackListViewModel>> GetByPlaylistId(Guid playlistId)
    {
        var query = new GetTracksByPlaylistIdQuery {PlaylistId = playlistId};
        if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Authorization"].ToString()))
        {
            var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Post([FromForm] CreateTrackDto createTrackDto)
    {
        var audioPath = await _fileService.Upload(createTrackDto.Audio, FileType.Audio);
        var command = _mapper.Map<CreateTrackCommand>(createTrackDto);
        if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Authorization"].ToString()))
        {
            var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
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
        var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(DeleteTrackDto deleteTrackDto)
    {
        var command = new DeleteTrackCommand
        {
            Id = deleteTrackDto.Id, 
            PlaylistId = deleteTrackDto.PlaylistId
        };
        if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Authorization"].ToString()))
        {
            var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
            command.UserId = userId;
        }
        await _mediator.Send(command);
        return Ok();
    }
}