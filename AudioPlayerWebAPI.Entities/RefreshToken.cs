using System.ComponentModel.DataAnnotations.Schema;

namespace AudioPlayerWebAPI.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string AccessToken { get; set; }
        public string RefToken { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expiration { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        [NotMapped]
        public bool IsActive => Expiration > DateTime.Now;
        
    }
}
