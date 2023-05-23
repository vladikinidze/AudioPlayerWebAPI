using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.Login
{
    public class LoginCommand : IRequest<AuthViewModel>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
