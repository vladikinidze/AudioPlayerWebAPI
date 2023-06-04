using MediatR;

namespace AudioPlayerWebAPI.UseCase.Errors;

public class ErrorCommand : IRequest<Unit>
{
    public string Text { get; set; }
}