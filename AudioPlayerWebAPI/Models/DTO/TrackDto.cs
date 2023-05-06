namespace AudioPlayerWebAPI.Models.DTO
{
    public class TrackDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The Title is required")]
        public string Title { get; set; } = string.Empty;

        public string? Text { get; set; }

        public bool Explicit { get; set; } = false;

        public Guid ParentPlaylistId { get; set; }
    }
}
