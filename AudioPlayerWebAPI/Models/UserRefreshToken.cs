namespace AudioPlayerWebAPI.Models
{
    public class UserRefreshToken
    {
        public Guid Id { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expiration { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        [NotMapped]
        public bool IsActive => Expiration > DateTime.Now;
        
    }
}
