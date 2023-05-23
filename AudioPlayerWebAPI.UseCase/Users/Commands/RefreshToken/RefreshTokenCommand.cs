using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.RefreshTokenCommand
{
    public class RefreshTokenCommand : IRequest<AuthViewModel>
    {
        public Guid UserId { get; set; }
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
