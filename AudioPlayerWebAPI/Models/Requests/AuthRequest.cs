namespace AudioPlayerWebAPI.Models.Requests
{
    public class AuthRequest
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
