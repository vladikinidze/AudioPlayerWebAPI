using AudioPlayerWebAPI.Models;
using AudioPlayerWebAPI.Services.FileService;
using AudioPlayerWebAPI.Services.UserTokenService;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.CreatePlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.DeletePlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.UpdatePlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Queries.GerPlaylistsByUserId;
using AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylists;
using AudioPlayerWebAPI.UseCase.ViewModels;
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
    public class PlaylistController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IFileService _fileService;
        private readonly IUserTokenService _tokenService;

        public PlaylistController(IMapper mapper, IMediator mediator, 
            IFileService fileService, IUserTokenService tokenService)
        {
            _mapper = mapper;
            _mediator = mediator;
            _fileService = fileService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Gets the list of playlists
        /// </summary>
        /// <returns>Return PlaylistListVm</returns>
        /// <response code="200">Success</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PlaylistListViewModel>> GetAll()
        {
            var query = new GetPlaylistsQuery();
            var vm = await _mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Gets the list of user's playlists by id
        /// </summary>
        /// <param name="userId">User Id (guid)</param>
        /// <returns>Return PlaylistListVm</returns>
        /// <response code="200">Success</response>
        /// <response code="404">NotFound</response>
        [HttpGet("{userId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PlaylistListViewModel>> GetByUserId(Guid userId)
        {
            var query = new GetPlaylistsByUserIdQuery {UserId = userId };
            var vm = await _mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Gets the playlist by id
        /// </summary>
        /// <param name="id">Playlist Id (guid)</param>
        /// <returns>Return PlaylistVm</returns>
        /// <response code="200">Success</response>
        /// <response code="404">NotFound</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
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
        /// <returns>Return Id (guid)</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> Post([FromForm] CreatePlaylistDto createPlaylistDto)
        {
            var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
            var imagePath = await _fileService.Upload(createPlaylistDto.Image);
            var command = _mapper.Map<CreatePlaylistCommand>(createPlaylistDto);
            command.UserId = userId;
            command.Image = imagePath;
            var playlistId = await _mediator.Send(command);
            return Ok(playlistId);
        }

        /// <summary>
        /// Update the playlist
        /// </summary>
        /// <param name="updatePlaylistDto">CreatePlaylistDto object</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not found</response>
        /// <response code="401">Unauthorized</response>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromForm] UpdatePlaylistDto updatePlaylistDto)
        {
            var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
            var imagePath = await _fileService.Upload(updatePlaylistDto.Image);
            var command = _mapper.Map<UpdatePlaylistCommand>(updatePlaylistDto);
            command.UserId = userId;
            command.Image = imagePath;
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Delete the playlist by id
        /// </summary>
        /// <param name="id">Playlst id (guid)</param>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        /// <response code="401">Unauthorized</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
            var command = new DeletePlaylistCommand { Id = id, UserId = userId};
            await _mediator.Send(command);
            return Ok();
        }
    }
}
