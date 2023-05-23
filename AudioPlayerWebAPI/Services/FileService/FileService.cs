using AudioPlayerWebAPI.UseCase.Exceptions;

namespace AudioPlayerWebAPI.Services.FileService
{
    public class FileService : IFileService
    {
        public async Task<string> Upload(IFormFile? file)
        {
            var extension = file.FileName.Split('.').Last();
            if (extension != ".jpg")
            {
                throw new NotAllowedFileException(".jpg images");
            }
            var filename = Guid.NewGuid() + extension;
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files\\Audio");
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            var path = Path.Combine(filepath, filename);
            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return filename;
        }
    }
}
