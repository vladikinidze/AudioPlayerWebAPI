using MediatR;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.UpdateTrack
{
    public class UpdateTrackCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Text { get; set; }
        public string Audio { get; set; }
        public bool Explicit { get; set; }
        public Guid PlaylistId { get; set; }
        public Guid UserId { get; set; }
    }
}
