using System.Reflection;
using System.Text;
using AudioPlayerWebAPI.Infrastructure;
using AudioPlayerWebAPI.Middleware;
using AudioPlayerWebAPI.Services.TokenService;
using AudioPlayerWebAPI.UseCase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
                services.AddApplication(); 
                services.AddDbConnection(builder.Configuration);
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen(options =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                });
                services.AddCors(option =>
                {
                    option.AddPolicy("AllowAll", policy =>
                    {
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                        policy.AllowAnyOrigin();
                    });
                });
                builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
                builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
                services.AddSingleton<ITokenService>(new TokenService());
                services.AddAuthorization();
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
                app.UseCustomExceptionHandler();
                app.UseRouting();
                app.UseHttpsRedirection();
                app.UseCors("AllowAll");
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseApiVersioning();
                app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin());
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
                app.MapControllers();
            }
        }
    }
}