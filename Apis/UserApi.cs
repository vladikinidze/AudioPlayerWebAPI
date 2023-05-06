using AudioPlayerWebAPI.Models.DTO;
using Microsoft.AspNetCore.Builder;
using static System.Net.Mime.MediaTypeNames;

namespace AudioPlayerWebAPI.Apis
{
    public class UserApi : IApi
    {
        private readonly IConfiguration _configuration;

        public UserApi(IConfiguration configuration)
        {
            _configuration = configuration;
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
        private async Task<IResult> Autenticate(UserDto userDto,
            ITokenService tokenService, IUserRepository repository)
        {
            var user = await repository.AutenticateUserAsync(userDto);
            if (user == null)
            {
                return Results.BadRequest("Invalid email or password");
            }

            var token = tokenService.BuildToken(_configuration["Jwt:Key"]!,
                _configuration["Jwt:Issuer"]!, user);

            return Results.Ok(token);
        }

        [AllowAnonymous]
        private async Task<IResult> Registerate([FromBody] UserDto userDto,
            ITokenService tokenService, IUserRepository repository)
        {
            var user = await repository.InsertUserAsync(userDto);
            await repository.SaveAsync();
            return Results.Created($"$users/{user.Id}", user.Id);
        }

        [Authorize]
        private async Task<IResult> GetById(Guid userId, IUserRepository repository) =>
                    await repository.GetUserByIdAsync(userId) is User user
                        ? Results.Ok(user)
                        : Results.NotFound();

        [Authorize]
        private async Task<IResult> Put([FromBody] User user, IUserRepository repository)
        {
            await repository.UpdateUserAsync(user);
            await repository.SaveAsync();
            return Results.Ok();
        }

        [Authorize]
        private async Task<IResult> Delete(Guid userId, IUserRepository repository)
        {
            await repository.DeleteUserAsync(userId);
            await repository.SaveAsync();
            return Results.Ok();
        }
    }
}
