namespace AudioPlayerWebAPI.Models.DTO
{
    public class TrackDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Text { get; set; }
        public string Audio { get; set; } = string.Empty;
        public bool Explicit { get; set; }
        public DateTime Created { get; set; }
        public Guid ParentPlaylistId { get; set; }
    }
}
