namespace AudioPlayerWebAPI.UseCase.Dtos;

public class TokenDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
}