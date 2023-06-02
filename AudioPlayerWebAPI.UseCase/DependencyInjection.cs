using System.Reflection;
using AudioPlayerWebAPI.UseCase.Behavior;
using AudioPlayerWebAPI.UseCase.Services.HashService;
using AudioPlayerWebAPI.UseCase.Services.TokenService;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AudioPlayerWebAPI.UseCase;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCase(this IServiceCollection services)
    {
        services.AddSingleton<IHashService>(new HashService());
        services.AddTransient<ITokenService, TokenService>();
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}