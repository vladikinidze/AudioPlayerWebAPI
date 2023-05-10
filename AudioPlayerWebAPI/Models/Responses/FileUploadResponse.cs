namespace AudioPlayerWebAPI.Models.Responses
{
    public class FileUploadResponse
    {
        public int Status { get; set; }
        public string Error { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
    }
}
