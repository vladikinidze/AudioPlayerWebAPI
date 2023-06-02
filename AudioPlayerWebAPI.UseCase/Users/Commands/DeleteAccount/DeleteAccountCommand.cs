using MediatR;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.DeleteAccount
{
    public class DeleteAccountCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public string Password { get; set; } = null!;
    }
}
