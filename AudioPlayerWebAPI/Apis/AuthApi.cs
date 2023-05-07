using AudioPlayerWebAPI.Migrations;

namespace AudioPlayerWebAPI.Apis
{
    public class AuthApi : IApi
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public AuthApi(
            IConfiguration configuration,
            IMapper mapper,
            IUserRepository repository,
            IValidator<RegisterDto> registerValidator, 
            IValidator<LoginDto> loginValidator, 
            ITokenService tokenService, 
            IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _mapper = mapper;
            _repository = repository;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public void Register(WebApplication application)
        {
            application.MapPost("/api/auth/login", Autenticate)
                .Produces<string>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            application.MapPost("/api/auth/register", Registerate)
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            application.MapPost("/api/auth/refreshToken", RefreshToken)
                .Produces<string>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }

        [AllowAnonymous]
        private async Task<IResult> Autenticate(LoginDto loginDto)
        {
            var validation = await _loginValidator.ValidateAsync(loginDto);
            if (!validation.IsValid)
            {
                return Results.ValidationProblem(validation.ToDictionary());
            }
            var user = await _repository.AutenticateUserAsync(loginDto);
            if (user == null)
            {
                return Results.BadRequest("Invalid email or password");
            }

            return Results.Ok(await BuildTokens(user));
        }

        [AllowAnonymous]
        private async Task<IResult> Registerate([FromBody] RegisterDto registerDto)
        {
            var validation = await _registerValidator.ValidateAsync(registerDto);
            if (!validation.IsValid)
            {
                return Results.ValidationProblem(validation.ToDictionary());
            }
            var user = await _repository.RegisterateUserAsync(registerDto);
            await _repository.SaveAsync();
            return Results.Created($"$users/{user.Id}", user.Id);
        }

        [AllowAnonymous]
        private async Task<IResult> RefreshToken([FromBody] AuthRequest request)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(request.Token);
            var validTo = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Expiration)!.Value;
            var refreshToken = await _refreshTokenRepository.GetAsync(request.Token, request.RefreshToken);
            if (refreshToken == null)
            {
                return Results.BadRequest("Invalid token.");
            }
            if (Convert.ToDateTime(validTo) > DateTime.Now)
            {
                return Results.BadRequest("Token not expired.");
            }
            if (!refreshToken.IsActive)
            {
                return Results.BadRequest("Refresh token expired.");
            }

            var userId = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var user = await _repository.GetUserByIdAsync(new Guid(userId));
            return Results.Ok(await BuildTokens(user));
        }

        private async Task<AuthResponse> BuildTokens(User user)
        {
            var accessToken = _tokenService.BuildToken(_configuration["Jwt:Key"]!,
                _configuration["Jwt:Issuer"]!, user);

            var refreshToken = _tokenService.BuildRefreshToken(user, accessToken);
            await _refreshTokenRepository.SetRefreshTokenAsync(refreshToken);
            await _refreshTokenRepository.SaveAsync();
            return new AuthResponse { RefreshToken = refreshToken.RefreshToken, Token = accessToken };
        }
    }
}
