using AudioPlayerWebAPI.Models;
using AudioPlayerWebAPI.Services.FileService;
using AudioPlayerWebAPI.Types;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.AddPlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.CreatePlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.DeletePlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.DeletePlaylistFromAdded;
using AudioPlayerWebAPI.UseCase.Playlists.Commands.UpdatePlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Queries.GerPlaylistsByUserId;
using AudioPlayerWebAPI.UseCase.Playlists.Queries.GetFavoritePlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylist;
using AudioPlayerWebAPI.UseCase.Playlists.Queries.GetPlaylists;
using AudioPlayerWebAPI.UseCase.Playlists.Queries.IsAddedPlaylist;
using AudioPlayerWebAPI.UseCase.Services.TokenService;
using AudioPlayerWebAPI.UseCase.Tracks.Commands.AddToFavoritePlaylist;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPlayerWebAPI.Controllers;

[ApiVersionNeutral]
[ApiController]
[Route("api/{version:apiVersion}/[controller]")]
public class PlaylistController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IFileService _fileService;
    private readonly ITokenService _tokenService;

    public PlaylistController(IMapper mapper, IMediator mediator, 
        IFileService fileService, ITokenService tokenService)
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
    public async Task<ActionResult<PlaylistListViewModel>> GetAll()
    {
        var query = new GetPlaylistsQuery();
        if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Authorization"].ToString()))
        {
            var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
            query.UserId = userId;
        }
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
    [HttpGet("User/{userId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<PlaylistListViewModel>> GetByUserId(Guid userId)
    {
        var query = new GetPlaylistsByUserIdQuery { UserId = userId };
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
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<PlaylistViewModel>> GetById(Guid id)
    {
        var query = new GetPlaylistQuery { Id = id };
        if (!string.IsNullOrEmpty(HttpContext.Request.Headers["Authorization"].ToString()))
        {
            var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
            query.UserId = userId;
        }
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
    public async Task<ActionResult<Guid>> Post([FromForm] CreatePlaylistDto createPlaylistDto)
    {
        var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
        var imagePath = await _fileService.Upload(createPlaylistDto.Image, FileType.Image);
        var command = _mapper.Map<CreatePlaylistCommand>(createPlaylistDto);
        command.UserId = userId;
        command.Image = imagePath;
        var playlistId = await _mediator.Send(command);
        return Ok(playlistId);
    }

    /// <summary>
    /// Update the playlist
    /// </summary>
    /// <param name="updatePlaylistDto">UpdatePlaylistDto object</param>
    /// <response code="200">Success</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Not found</response>
    /// <response code="401">Unauthorized</response>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdatePlaylistDto updatePlaylistDto)
    {
        var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
        var command = _mapper.Map<UpdatePlaylistCommand>(updatePlaylistDto);
        if (updatePlaylistDto.Image != null)
        {
            var imagePath = await _fileService.Upload(updatePlaylistDto.Image, FileType.Image);
            command.Image = imagePath;
        }
        if (updatePlaylistDto.EmptyImage)
        {
            command.Image = "548864f8-319e-40ac-9f9b-a31f65ccb902.jpg";
        }
        command.UserId = userId;
        await _mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Delete the playlist by id
    /// </summary>
    /// <param name="id">Playlist id (guid)</param>
    /// <response code="200">Success</response>
    /// <response code="404">Not found</response>
    /// <response code="401">Unauthorized</response>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
        var command = new DeletePlaylistCommand { Id = id, UserId = userId};
        await _mediator.Send(command);
        return Ok();
    }
    
    /// <summary>
    /// Add the playlist to user's playlists
    /// </summary>
    /// <param name="id">playlist id</param>
    /// <response code="200">Success</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost("User/AddPlaylist/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> AddPlaylist(Guid id)
    {
        var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
        var command = new AddPlaylistCommand
        {
            UserId = userId,
            PlaylistId = id
        };
        await _mediator.Send(command);
        return Ok();
    }
    
    /// <summary>
    /// Delete the playlist to user's playlists
    /// </summary>
    /// <param name="id">playlist id</param>
    /// <response code="200">Success</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized</response>
    [HttpDelete("User/DeletePlaylist/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeletePlaylist(Guid id)
    {
        var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
        var command = new DeletePlaylistFromAddedCommand()
        {
            UserId = userId,
            PlaylistId = id
        };
        await _mediator.Send(command);
        return Ok();
    }
    
    /// <summary>
    /// The playlist has been added by the user or not
    /// </summary>
    /// <param name="playlistId">Playlist Id (guid)</param>
    /// <returns>Return bool</returns>
    /// <response code="200">Success</response>
    /// <response code="404">NotFound</response>
    [HttpGet("User/IsAdded/{playlistId:guid}")]
    [Authorize]
    public async Task<ActionResult<bool>> IsAdded(Guid playlistId)
    {
        var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
        var query = new IsAddedPlaylistQuery() { UserId = userId, PlaylistId = playlistId};
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    /// <summary>
    /// Get the playlist contains favorite user's tracks
    /// </summary>
    /// <returns>Return bool</returns>
    /// <response code="200">Success</response>
    /// <response code="404">NotFound</response>
    [HttpGet("User/Favorite")]
    [Authorize]
    public async Task<ActionResult<bool>> GetFavoritePlaylist()
    {
        var userId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"].ToString()).UserId;
        var query = new GetFavoritePlaylistQuery{ UserId = userId};
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}