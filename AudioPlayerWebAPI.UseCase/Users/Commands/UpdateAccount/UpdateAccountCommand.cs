using MediatR;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.UpdateAccount
{
    public class UpdateAccountCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Image { get; set; }
        //public string Password { get; set; } = null!;
    }
}
