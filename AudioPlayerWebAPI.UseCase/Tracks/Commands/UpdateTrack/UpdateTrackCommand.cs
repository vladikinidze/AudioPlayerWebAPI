using MediatR;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.UpdateTrack
{
    public class UpdateTrackCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public double Duration { get; set; }
        public string Audio { get; set; } = null!;
        public bool Explicit { get; set; }
        public Guid PlaylistId { get; set; }
        public Guid UserId { get; set; }
    }
}
