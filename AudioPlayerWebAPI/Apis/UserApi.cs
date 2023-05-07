namespace AudioPlayerWebAPI.Apis
{
    public class UserApi : IApi
    {
        private readonly IUserRepository _repository;
        private readonly IValidator<User> _validator;

        public UserApi(
            IUserRepository repository,
            IValidator<User> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public void Register(WebApplication application)
        {
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
