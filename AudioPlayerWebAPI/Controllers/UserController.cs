using AudioPlayerWebAPI.Models;
using AudioPlayerWebAPI.Services.FileService;
using AudioPlayerWebAPI.Services.UserTokenService;
using AudioPlayerWebAPI.UseCase.Users.Commands.DeleteAccount;
using AudioPlayerWebAPI.UseCase.Users.Commands.Login;
using AudioPlayerWebAPI.UseCase.Users.Commands.RefreshTokenCommand;
using AudioPlayerWebAPI.UseCase.Users.Commands.Register;
using AudioPlayerWebAPI.UseCase.Users.Commands.UpdateAccount;
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
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IFileService _fileService;
        private readonly IUserTokenService _tokenService;

        public UserController(IMapper mapper, IMediator mediator, IFileService fileService, IUserTokenService tokenService)
        {
            _mapper = mapper;
            _mediator = mediator;
            _fileService = fileService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Authentication
        /// </summary>
        /// <param name="loginDto">LoginDto object</param>
        /// <response code="200">Success</response>
        /// <response code="404">Bad request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthViewModel>> AuthenticateUser([FromBody] LoginDto loginDto)
        {
            var command = _mapper.Map<LoginCommand>(loginDto);
            var userId = await _mediator.Send(command);
            return Ok(userId);
        }

        /// <summary>
        /// Registration
        /// </summary>
        /// <param name="registerDto">RegisterDto object</param>
        /// <response code="200">Success</response>
        /// <response code="404">Bad request</response>
        /// <response code="401">Unauthorized</response>
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
        /// <response code="404">Bad request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthViewModel>> RefreshToken([FromBody]RefreshTokenDto refreshTokenDto)
        {
            var command = _mapper.Map<RefreshTokenCommand>(refreshTokenDto);
            var vm = await _mediator.Send(command);
            return Ok(vm);
        }

        /// <summary>
        /// Update user info
        /// </summary>
        /// <param name="updateUserDto">UpdateUserDto object</param>
        /// <response code="200">Success</response>
        /// <response code="404">Bad request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UpdateUserDto updateUserDto)
        {
            var userId = _tokenService.GetUserId(HttpContext.Request.Headers["Authorization"].ToString());
            var imagePath = await _fileService.Upload(updateUserDto.Image);
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
        /// <response code="404">Bad request</response>
        /// <response code="401">Unauthorized</response>
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
}
