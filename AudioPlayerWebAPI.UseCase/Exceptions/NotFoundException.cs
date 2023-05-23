namespace AudioPlayerWebAPI.UseCase.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity, string key)
            : base($"Entity {entity} ({key}) not found.")
        { }
    }
}
