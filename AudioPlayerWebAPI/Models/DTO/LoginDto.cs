namespace AudioPlayerWebAPI.Models.DTO
{
    public class LoginDto
    {
        [Required(ErrorMessage = "The Email is required")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
