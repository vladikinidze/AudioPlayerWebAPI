﻿namespace AudioPlayerWebAPI.UseCase.Exceptions
{
    public class RefreshTokenException : Exception
    {
        public RefreshTokenException(string message) 
            : base(message)
        { }
    }
}
