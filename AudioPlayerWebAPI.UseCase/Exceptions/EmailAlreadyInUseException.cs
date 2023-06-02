namespace AudioPlayerWebAPI.UseCase.Exceptions
{
    public class EmailAlreadyInUseException : Exception
    {
        public EmailAlreadyInUseException() 
            : base("This email already in use")
        { } 
    }
}
