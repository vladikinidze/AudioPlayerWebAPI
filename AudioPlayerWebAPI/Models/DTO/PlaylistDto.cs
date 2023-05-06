namespace AudioPlayerWebAPI.Models.DTO
{
    public class PlaylistDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The Title is required")]
        public string Title { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public DateTime Created { get; set; }
    }
}
