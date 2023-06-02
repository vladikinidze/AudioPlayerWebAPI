using MediatR;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.CreateTrack
{
    public class CreateTrackCommand : IRequest<Guid>
    {
        public string Title { get; set; } = null!;
        public string? Text { get; set; }
        public bool Explicit { get; set; }
        public string Audio { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid PlaylistId { get; set; }
    }
}
