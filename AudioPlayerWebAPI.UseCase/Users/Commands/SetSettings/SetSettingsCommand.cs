using MediatR;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.SetSettings;

public class SetSettingsCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public bool Explicit { get; set; }
}