using FluentValidation;

namespace AudioPlayerWebAPI.UseCase.Users.Queries
{
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(query => query.Id).NotEqual(Guid.Empty);
        }
    }
}
