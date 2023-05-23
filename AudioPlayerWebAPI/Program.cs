using System.Reflection;
using System.Text;
using AudioPlayerWebAPI.Infrastructure;
using AudioPlayerWebAPI.Middleware;
using AudioPlayerWebAPI.Services.FileService;
using AudioPlayerWebAPI.Services.UserTokenService;
using AudioPlayerWebAPI.UseCase;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AudioPlayerWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            RegisterServices(builder.Services);
            var app = builder.Build();
            Configure(app);
            app.Run();

            void RegisterServices(IServiceCollection services)
            {
                services.AddAutoMapper(config =>
                {
                    config.AddProfile(new MappingProfile(Assembly.GetExecutingAssembly()));
                    config.AddProfile(new MappingProfile(typeof(IAudioPlayerDbContext).Assembly));
                });
                services.AddApplication(); 
                services.AddDbConnection(builder.Configuration);
                services.AddControllers();
                services.AddCors(option =>
                {
                    option.AddPolicy("AllowAll", policy =>
                    {
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                        policy.AllowAnyOrigin();
                    });
                });
                services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
                services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
                services.AddSingleton<IFileService>(new FileService());
                services.AddSingleton<IUserTokenService>(new UserTokenService());
                services.AddSwaggerGen(options =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                });
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new()
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = builder.Configuration["Jwt:Issuer"],
                            ValidAudience = builder.Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                        };
                    });
                services.AddAuthorization();
                services.AddApiVersioning();
            }

            void Configure(WebApplication app)
            {
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    using var scope = app.Services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AudioPlayerDbContext>();
                    db.Database.EnsureCreated();
                }
                app.UseSwagger();
                app.UseSwaggerUI(config =>
                {
                    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        config.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                        config.RoutePrefix = string.Empty;
                    }
                });
                app.UseCustomExceptionHandler();
                app.UseRouting();
                app.UseHttpsRedirection();
                app.UseCors("AllowAll");
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseApiVersioning();
                app.MapControllers();
            }
        }
    }
}