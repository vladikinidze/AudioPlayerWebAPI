using AudioPlayerWebAPI.Types;

namespace AudioPlayerWebAPI.Services.FileService
{
    public interface IFileService
    {
        Task<string> Upload(IFormFile? file, FileType type);
    }
}
