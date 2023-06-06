namespace AudioPlayerWebAPI.UseCase.Exceptions
{
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException(string message) 
            : base(message) { }
    }
}
