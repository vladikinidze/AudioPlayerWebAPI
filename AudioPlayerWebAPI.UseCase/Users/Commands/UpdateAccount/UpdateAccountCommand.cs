using MediatR;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.UpdateAccount
{
    public class UpdateAccountCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Image { get; set; }
        public string Password { get; set; }
    }
}
