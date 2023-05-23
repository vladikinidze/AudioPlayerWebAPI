using AudioPlayer.WebAPI.Models;
using AudioPlayerWebAPI.Models;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.CreateTrack;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteTrack;
using AudioPlayerWebAPI.UseCase.Tracks.Queries.GetTracks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AudioPlayerWebAPI.Controllers
{
    [ApiVersionNeutral]
    [Produces("application/json")]
    [Route("api/{version:apiVersion}/[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public TrackController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the list of tracks by playlistId
        /// </summary>
        /// <param name="playlistId">Playlist Id</param>
        /// <returns>Return TrackListVm</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [HttpGet]
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
        /// <param name="audio">Playlist image</param>
        /// <param name="createTrackDto">CreateTrackDto object</param>
        /// <returns>Return Id (guid)</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> Post([FromForm] CreateTrackDto createTrackDto, IFormFile audio)
        {
            var command = _mapper.Map<CreateTrackCommand>(createTrackDto);
            var playlistId = await _mediator.Send(command);
            return Ok(playlistId);
        }

        /// <summary>
        /// Update the track
        /// </summary>
        /// <param name="audio">Playlist image</param>
        /// <param name="updateTrackDto">UpdateTrackDto object</param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> Update([FromForm] UpdateTrackDto updateTrackDto, IFormFile audio)
        {
            var command = _mapper.Map<CreateTrackCommand>(updateTrackDto);
            await _mediator.Send(command);
            return Ok();
        }


        /// <summary>
        /// Delete the track by id
        /// </summary>
        /// <param name="id">Track id (guid)</param>
        /// <param name="playlistId">Playlist id (guid)</param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id, Guid playlistId)
        {
            var command = new DeleteTrackCommand { Id = id, PlaylistId = playlistId};
            await _mediator.Send(command);
            return Ok();
        }
    }
}
