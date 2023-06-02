using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Users.Queries.GetUserQuery
{
    public class GetUserQuery : IRequest<UserViewModel>
    {
        public Guid Id { get; set; }
    }
}
