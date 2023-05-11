using AudioPlayerWebAPI.Models;

namespace AudioPlayerWebAPI.Apis
{
    public class UserApi : IApi
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _validator;
        private readonly IMapper _mapper;

        public UserApi(IUserRepository userRepository, 
            IValidator<User> validator, IMapper mapper)
        {
            _userRepository = userRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public void Register(WebApplication application)
        {
            application.MapGet("/api/users/{userId}", GetById)
                .Produces<User>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            application.MapPut("/api/users", Put)
                .Accepts<User>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            application.MapDelete("/api/users/{userId}", Delete)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
        }


        [Authorize]
        private async Task<IResult> GetById(Guid userId) =>
                    _mapper.Map<UserDto>(await _userRepository.GetUserByIdAsync(userId)) is UserDto user
                        ? Results.Ok(user)
                        : Results.NotFound();

        [Authorize]
        private async Task<IResult> Put([FromBody] User user)
        {
            var validation = await _validator.ValidateAsync(user);
            if (!validation.IsValid)
            {
                return Results.ValidationProblem(validation.ToDictionary());
            }
            await _userRepository.UpdateUserAsync(user);
            await _userRepository.SaveAsync();
            return Results.Ok();
        }

        [Authorize]
        private async Task<IResult> Delete(Guid userId, string password)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user.Password != Hash.GetSha1Hash(password))
            {
                return Results.Forbid();
            }
            await _userRepository.DeleteUserAsync(userId);
            await _userRepository.SaveAsync();
            return Results.Ok();
        }
    }
}
