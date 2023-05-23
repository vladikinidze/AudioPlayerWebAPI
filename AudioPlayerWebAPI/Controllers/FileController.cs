using AudioPlayerWebAPI.Services.UserTokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPlayerWebAPI.Controllers
{
    [ApiVersionNeutral]
    [ApiController]
    [Route("api/{version:apiVersion}/[controller]")]
    public class FileController : Controller
    {
        /// <summary>
        /// Get file by name
        /// </summary>
        /// <returns>Return PlaylistListVm</returns>
        /// <param name="filename">FileName</param>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [AllowAnonymous]
        [HttpGet("{filename}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByName(string filename)
        {
            var contentType = "image/jpeg";
            var folder = "Image";
            if (filename.Contains(".mp3"))
            {
                contentType = "audio/mpeg";
                folder = "Audio";
            }

            var path = Directory.GetCurrentDirectory() + "\\Upload\\Files\\" + folder + "\\" + filename;

            if (!System.IO.File.Exists(path))
            {
                return NotFound("File not founf");
            }
            var file = System.IO.File.OpenRead(path);
            return File(file, contentType);
        }

        /// <summary>
        /// Download file by name
        /// </summary>
        /// <returns>Return PlaylistListVm</returns>
        /// <param name="filename">FileName</param>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [Authorize]
        [HttpGet("download/{filename}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string filename)
        {
            var extension = "." + filename.Split('.').Last();
            var filepath = Path.Combine(Directory.GetCurrentDirectory(),
                "Upload\\Files", extension == ".mp3" ? "Audio" : "Image", filename);
            if (!System.IO.File.Exists(filepath))
            {
                return NotFound("File not found");
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, "application/octet-stream", Path.GetFileName(filepath));
        }
    }
}
