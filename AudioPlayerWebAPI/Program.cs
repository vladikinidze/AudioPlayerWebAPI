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

            using var scope = app.Services.CreateScope();
            var apis = scope.ServiceProvider.GetServices<IApi>();
            foreach (var api in apis)
            {
                if (api is null) throw new InvalidProgramException("Api not found");
                api.Register(app);
            }

            app.Run();

            void RegisterServices(IServiceCollection services)
            {
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                },
                                Scheme = "Oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                                
                            },
                            new string[]{}
                        }
                    });
                });
                services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                services.AddDbContext<AudioPlayerDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
                services.AddScoped<IValidator<User>, UserValidator>();
                services.AddScoped<IValidator<RegisterDto>, RegisterUserDtoValidator>();
                services.AddScoped<IValidator<LoginDto>, LoginUserDtoValidator>();
                services.AddScoped<IValidator<TrackDto>, TrackDtoValidator>();
                services.AddScoped<IValidator<Playlist>, PlaylistValidator>();
                services.AddScoped<ITrackRepository, TrackRepository>();
                services.AddScoped<IPlaylistRepository, PlaylistRepository>();
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
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
                                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                        };
                    });

                builder.Services.AddTransient<IApi, TrackApi>();
                builder.Services.AddTransient<IApi, PlaylistApi>();
                builder.Services.AddTransient<IApi, FileApi>();
                builder.Services.AddTransient<IApi, UserApi>();
                builder.Services.AddTransient<IApi, AuthApi>();
            }

            void Configure(WebApplication app)
            {
                app.UseAuthentication();
                app.UseAuthorization();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(options =>
                    {
                        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                        options.RoutePrefix = string.Empty;
                    });
                    app.UseDeveloperExceptionPage();
                    //using var scope = app.Services.CreateScope();
                    //var db = scope.ServiceProvider.GetRequiredService<AudioPlayerDbContext>();
                    //db.Database.EnsureCreated();
                }

                app.UseHttpsRedirection();
            }
        }
    }
}