namespace AudioPlayerWebAPI.UseCase.Exceptions
{
    public class NotAllowedFileException : Exception
    {
        public NotAllowedFileException(string fileType)
            : base($"Only {fileType} are allowed")
        { }
    }
}
