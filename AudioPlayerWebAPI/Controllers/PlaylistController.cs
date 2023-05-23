using AudioPlayerWebAPI.Models;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.CreatePlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.DeletePlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.UpdatePlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylists;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AudioPlayerWebAPI.Controllers
{
    [ApiVersionNeutral]
    [Produces("application/json")]
    [Route("api/{version:apiVersion}/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public PlaylistController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the list of playlists
        /// </summary>
        /// <returns>Return PlaylistListVm</returns>
        /// <response code="200">Success</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PlaylistListViewModel>> GetAll()
        {
            var query = new GetPlaylistsQuery();
            var vm = await _mediator.Send(query);
            return Ok(vm);
        }


        /// <summary>
        /// Gets the playlist by id
        /// </summary>
        /// <param name="id">Playlist Id (guid)</param>
        /// <param name="userId">User Id (guid)</param>
        /// <returns>Return PlaylistVm</returns>
        /// <response code="200">Success</response>
        /// <response code="404">NotFound</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PlaylistViewModel>> GetById(Guid id)
        {
            var query = new GetPlaylistQuery { Id = id};
            var vm = await _mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Create the playlist
        /// </summary>
        /// <param name="createPlaylistDto">CreatePlaylistDto object</param>
        /// <param name="image">Playlist image</param>
        /// <returns>Return Id (guid)</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> Post([FromForm] CreatePlaylistDto createPlaylistDto, IFormFile image)
        {
            var command = _mapper.Map<CreatePlaylistCommand>(createPlaylistDto);
            var playlistId = await _mediator.Send(command);
            return Ok(playlistId);
        }


        /// <summary>
        /// Update the playlist
        /// </summary>
        /// <param name="updatePlaylistDto">CreatePlaylistDto object</param>
        /// <param name="image">Playlist image</param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromForm] UpdatePlaylistDto updatePlaylistDto, IFormFile image)
        {
            var command = _mapper.Map<UpdatePlaylistCommand>(updatePlaylistDto);
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Delete the playlist by id
        /// </summary>
        /// <param name="id">Playlst id (guid)</param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeletePlaylistCommand { Id = id};
            await _mediator.Send(command);
            return Ok();
        }
    }
}
