namespace AudioPlayerWebAPI.Infrastructure;

public class DbInitializer
{
    public static void Initialize(AudioPlayerDbContext context)
    {
        context.Database.EnsureCreated();
    }
}