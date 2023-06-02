using AudioPlayerWebAPI.Types;
using AudioPlayerWebAPI.UseCase.Exceptions;

namespace AudioPlayerWebAPI.Services.FileService
{
    public class FileService : IFileService
    {
        public async Task<string> Upload(IFormFile? file, FileType type)
        {
            if (file == null)
            {
                return type == FileType.Image ? "548864f8-319e-40ac-9f9b-a31f65ccb902.jpg" : string.Empty;
            }
            var extension = file.FileName.Split('.').Last();
            if (extension != "jpg" && extension != "mp3")
            {
                throw new NotAllowedFileException(".jpg images and .mp3 audio");
            }
            var filename = Guid.NewGuid() + "." + extension;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files\\", extension == "jpg" ? "Image" : "Audio");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var path = Path.Combine(filePath, filename);
            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return filename;
        }
    }
}
