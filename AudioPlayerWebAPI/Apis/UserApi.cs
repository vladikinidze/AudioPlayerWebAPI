using AudioPlayerWebAPI.Models;
using AudioPlayerWebAPI.Models.DTO;

namespace AudioPlayerWebAPI.Apis
{
    public class UserApi : IApi
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _repository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;

        public UserApi(IConfiguration configuration, 
            IUserRepository repository,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService)
        {
            _configuration = configuration;
            _repository = repository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
        }

        public void Register(WebApplication application)
        {
            application.MapGet("/api/users/{id}", GetById)
                .Produces<User>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            application.MapPost("/api/login", Autenticate)
                .Produces<string>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            application.MapPost("/api/register", Registerate)
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            application.MapPost("/api/refreshToken", RefreshToken)
                .Produces<string>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            application.MapPut("/api/users", Put)
                .Accepts<User>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            application.MapDelete("/api/users/{id}", Delete)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
        }

        [AllowAnonymous]
        private async Task<IResult> Autenticate(UserDto userDto)
        {
            var user = await _repository.AutenticateUserAsync(userDto);
            if (user == null)
            {
                return Results.BadRequest("Invalid email or password");
            }

            return Results.Ok(await BuildTokens(user));
        }

        [AllowAnonymous]
        private async Task<IResult> Registerate([FromBody] UserDto userDto)
        {
            var user = await _repository.RegisterateUserAsync(userDto);
            await _repository.SaveAsync();
            return Results.Created($"$users/{user.Id}", user.Id);
        }

        [AllowAnonymous]
        private async Task<IResult> RefreshToken([FromBody] AuthRequest request)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(request.Token);
            var validTo = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Expiration)!.Value;
            var refreshToken = await _refreshTokenRepository.GetAsync(request.Token, request.RefreshToken);
            if (refreshToken == null)
            {
                return Results.BadRequest("Invalid token.");
            }
            if (Convert.ToDateTime(validTo) > DateTime.Now)
            {
                return Results.BadRequest("Token not expired.");
            }
            if (!refreshToken.IsActive)
            {
                return Results.BadRequest("Refresh token expired.");
            }

            var userId = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var user = await _repository.GetUserByIdAsync(new Guid(userId));
            return Results.Ok(await BuildTokens(user));
        }

        [Authorize]
        private async Task<IResult> GetById(Guid userId) =>
                    await _repository.GetUserByIdAsync(userId) is User user
                        ? Results.Ok(user)
                        : Results.NotFound();

        [Authorize]
        private async Task<IResult> Put([FromBody] User user)
        {
            await _repository.UpdateUserAsync(user);
            await _repository.SaveAsync();
            return Results.Ok();
        }

        [Authorize]
        private async Task<IResult> Delete(Guid userId)
        {
            await _repository.DeleteUserAsync(userId);
            await _repository.SaveAsync();
            return Results.Ok();
        }

        private async Task<AuthResponse> BuildTokens(User user)
        {
            var accessToken = _tokenService.BuildToken(_configuration["Jwt:Key"]!,
                _configuration["Jwt:Issuer"]!, user);

            var refreshToken = _tokenService.BuildRefreshToken(user, accessToken);
            await _refreshTokenRepository.SetRefreshTokenAsync(refreshToken);
            await _refreshTokenRepository.SaveAsync();
            return new AuthResponse { RefreshToken = refreshToken.RefreshToken, Token = accessToken };
        }
    }
}
