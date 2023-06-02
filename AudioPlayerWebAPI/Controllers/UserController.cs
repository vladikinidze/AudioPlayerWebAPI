using AudioPlayerWebAPI.Models;
using AudioPlayerWebAPI.Services.FileService;
using AudioPlayerWebAPI.Services.UserTokenService;
using AudioPlayerWebAPI.Types;
using AudioPlayerWebAPI.UseCase.Users.Commands.DeleteAccount;
using AudioPlayerWebAPI.UseCase.Users.Commands.Login;
using AudioPlayerWebAPI.UseCase.Users.Commands.RefreshToken;
using AudioPlayerWebAPI.UseCase.Users.Commands.Register;
using AudioPlayerWebAPI.UseCase.Users.Commands.SetSettings;
using AudioPlayerWebAPI.UseCase.Users.Commands.UpdateAccount;
using AudioPlayerWebAPI.UseCase.Users.Queries.GetUserQuery;
using AudioPlayerWebAPI.UseCase.Users.Queries.GetUserSettingsQuery;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPlayerWebAPI.Controllers;

[ApiVersionNeutral]
[ApiController]
[Route("api/{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IFileService _fileService;
    private readonly IUserTokenService _tokenService;

    public UserController(IMapper mapper, IMediator mediator, 
        IFileService fileService, IUserTokenService tokenService)
    {
        _mapper = mapper;
        _mediator = mediator;
        _fileService = fileService;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Get user by Id
    /// </summary>
    /// <param name="id">User id (guid)</param>
    /// <response code="200">Success</response>
    /// <response code="404">Not found</response>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<UserViewModel>> GetById(Guid id)
    {
        var command = new GetUserQuery { Id = id };
        var vm = await _mediator.Send(command);
        return Ok(vm);
    }

    /// <summary>
    /// Authentication
    /// </summary>
    /// <param name="loginDto">LoginDto object</param>
    /// <response code="200">Success</response>
    /// <response code="400">Not found</response>
    /// <response code="404">Bad request</response>
    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthViewModel>> AuthenticateUser([FromBody] LoginDto loginDto)
    {
        var command = _mapper.Map<LoginCommand>(loginDto);
        var vm = await _mediator.Send(command);
        return Ok(vm);
    }

    /// <summary>
    /// Registration
    /// </summary>
    /// <param name="registerDto">RegisterDto object</param>
    /// <response code="200">Success</response>
    /// <response code="400">Not found</response>
    /// <response code="404">Bad request</response>
    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
    {
        var command = _mapper.Map<RegisterCommand>(registerDto);
        var userId = await _mediator.Send(command);
        return Ok(userId);
    }

    /// <summary>
    /// Refresh access and refresh tokens
    /// </summary>
    /// <param name="refreshTokenDto">RefreshTokenDto object</param>
    /// <response code="200">Success</response>
    /// <response code="404">Not found</response>
    /// <response code="400">Bad request</response>
    [HttpPost("RefreshToken")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthViewModel>> RefreshToken([FromBody]RefreshTokenDto refreshTokenDto)
    {
        var command = _mapper.Map<RefreshTokenCommand>(refreshTokenDto);
        var vm = await _mediator.Send(command);
        return Ok(vm);
    }
    
    /// <summary>
    /// Get the user settings
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Not found</response>
    /// <response code="404">Bad request</response>
    [HttpGet("Settings")]
    [Authorize]
    public async Task<IActionResult> GetSettings()
    {
        var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
        var query = new GetUserSettingsQuery { UserId = userId };
        var vm = await _mediator.Send(query);
        return Ok(vm);
    }
    
    /// <summary>
    /// Set the user settings
    /// </summary>
    /// <param name="settingsDto">SettingsDto object</param>
    /// <response code="200">Success</response>
    /// <response code="400">Not found</response>
    /// <response code="404">Bad request</response>
    [HttpPost("Settings")]
    [Authorize]
    public async Task<IActionResult> SetSettings([FromBody] SettingsDto settingsDto)
    {
        var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
        var command = _mapper.Map<SetSettingsCommand>(settingsDto);
        command.UserId = userId;
        await _mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Update user info
    /// </summary>
    /// <param name="updateUserDto">UpdateUserDto object</param>
    /// <response code="200">Success</response>
    /// <response code="404">Not found</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized</response>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateUserDto updateUserDto)
    {
        var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
        var imagePath = await _fileService.Upload(updateUserDto.Image, FileType.Image);
        var command = _mapper.Map<UpdateAccountCommand>(updateUserDto);
        command.Image = imagePath;
        command.Id = userId;
        await _mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Delete account
    /// </summary>
    /// <param name="deleteUserDto">UpdateUserDto object</param>
    /// <response code="200">Success</response>
    /// <response code="400">Bad request</response>
    /// <response code="404">Not found</response>
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete([FromBody] DeleteUserDto deleteUserDto)
    {
        var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
        var command = _mapper.Map<DeleteAccountCommand>(deleteUserDto);
        command.UserId = userId;
        await _mediator.Send(command);
        return Ok();
    }
}