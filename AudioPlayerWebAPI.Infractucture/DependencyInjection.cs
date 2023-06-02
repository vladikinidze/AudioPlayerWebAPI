using AudioPlayerWebAPI.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AudioPlayerWebAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDbConnection(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:DefaultConnection"];
        services.AddDbContext<AudioPlayerDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddScoped<IAudioPlayerDbContext>(provider => 
            provider.GetService<AudioPlayerDbContext>()!);
        return services;
    }
}