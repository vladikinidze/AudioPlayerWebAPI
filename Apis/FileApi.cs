namespace AudioPlayerWebAPI.Apis
{
    public class FileApi : IApi
    {
        public void Register(WebApplication application)
        {
            application.MapPost("/api/upload", Post)
                .Produces<string>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError);
            application.MapGet("/api/download", Get)
                .Produces<string>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }


        [Authorize]
        private async Task<IResult> Post(IFormFile file)
        {
            try
            {
                string filename = "";
                var extension = "." + file.FileName.Split('.').Last();
                if (extension != ".jpg" && extension != ".png" && extension != ".jpeg" && extension != ".mp3") {
                    return Results.BadRequest("Only .mp3, .jpg, .png files");
                }
                filename = Guid.NewGuid() + extension;
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files\\Static", extension == ".mp3" ? "Audio" : "Image");
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                var exactpath = Path.Combine(filepath, filename);
                await using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return Results.Ok(filename);
            }
            catch (Exception)
            {
                return Results.Problem(statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        [Authorize]
        private async Task<IResult> Get(string filename)
        {
            var extension = "." + filename.Split('.').Last();
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files\\Static", extension == ".mp3" ? "Audio" : "Image", filename);
            if (!File.Exists(filepath))
            {
                return Results.NotFound("File not found");
            }
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filename, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = await File.ReadAllBytesAsync(filepath);
            return Results.File(bytes, contentType, Path.GetFileName(filepath));
        }

    }
}