using MediatR;

namespace AudioPlayerWebAPI.UseCase.Tracks.Commands.DeleteTrack
{
    public class DeleteTrackCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid PlaylistId { get; set; }
        public Guid UserId { get; set; }
    }
}
