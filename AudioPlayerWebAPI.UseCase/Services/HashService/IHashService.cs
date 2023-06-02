namespace AudioPlayerWebAPI.UseCase.Services.HashService;

public interface IHashService
{
    string GetSha1Hash(string text);
}