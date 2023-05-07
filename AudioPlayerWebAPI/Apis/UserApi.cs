namespace AudioPlayerWebAPI.Apis
{
    public class UserApi : IApi
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _repository;
        private readonly ITokenService _tokenService;

        public UserApi(IConfiguration configuration, IUserRepository repository, ITokenService tokenService)
        {
            _configuration = configuration;
            _repository = repository;
            _tokenService = tokenService;
        }

        public void Register(WebApplication application)
        {
            application.MapPost("/api/login", Autenticate)
                .Produces<string>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            application.MapPost("/api/register", Registerate)
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            application.MapGet("/api/users/{id}", GetById)
                .Produces<User>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

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

            var token = _tokenService.BuildToken(_configuration["Jwt:Key"]!,
                _configuration["Jwt:Issuer"]!, user);

            return Results.Ok(token);
        }

        [AllowAnonymous]
        private async Task<IResult> Registerate([FromBody] UserDto userDto)
        {
            var user = await _repository.InsertUserAsync(userDto);
            await _repository.SaveAsync();
            return Results.Created($"$users/{user.Id}", user.Id);
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
    }
}
