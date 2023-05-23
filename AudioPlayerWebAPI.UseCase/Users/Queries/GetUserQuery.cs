using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Users.Queries
{
    public class GetUserQuery : IRequest<UserViewModel>
    {
        public Guid Id { get; set; }
    }
}
