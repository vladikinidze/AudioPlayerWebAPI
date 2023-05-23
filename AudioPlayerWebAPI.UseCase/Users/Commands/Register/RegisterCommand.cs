using MediatR;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.Register
{
    public class RegisterCommand : IRequest<Guid>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
