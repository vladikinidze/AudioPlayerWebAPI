using AudioPlayerWebAPI.Models;
using AudioPlayerWebAPI.Services.FileService;
using AudioPlayerWebAPI.Services.UserTokenService;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.CreateTrack;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteTrack;
using AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPlayerWebAPI.Controllers
{
    [ApiVersionNeutral]
    [ApiController]
    [Produces("application/json")]
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
            var query = new GetTracksQuery {PlaylistId = playlistId};
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
            var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
            var audioPath = await _fileService.Upload(createTrackDto.Audio);
            var command = _mapper.Map<CreateTrackCommand>(createTrackDto);
            command.Audio = audioPath;
            command.UserId = userId;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> Update([FromForm] UpdateTrackDto updateTrackDto)
        {
            var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
            var audioPath = await _fileService.Upload(updateTrackDto.Audio);
            var command = _mapper.Map<CreateTrackCommand>(updateTrackDto);
            command.Audio = audioPath;
            command.UserId = userId;
            await _mediator.Send(command);
            return Ok();
        }


        /// <summary>
        /// Delete the track by id
        /// </summary>
        /// <param name="deleteTrackDto">dDeleteTrackDto object</param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(DeleteTrackDto deleteTrackDto)
        {
            var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
            var command = new DeleteTrackCommand
            {
                UserId = userId, 
                Id = deleteTrackDto.Id, 
                PlaylistId = deleteTrackDto.PlaylistId
            };
            await _mediator.Send(command);
            return Ok();
        }
    }
}
