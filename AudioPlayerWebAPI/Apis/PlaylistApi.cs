namespace AudioPlayerWebAPI.Apis
{
    public class PlaylistApi : IApi
    {
        private readonly IPlaylistRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly ITrackRepository _trackRepository;
        private readonly IValidator<Playlist> _validator;
        private readonly IMapper _mapper;

        public PlaylistApi(
            IPlaylistRepository repository, 
            IUserRepository userRepository, 
            ITrackRepository trackRepository,
            IValidator<Playlist> validator, IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _trackRepository = trackRepository;
            _validator = validator;
            _mapper = mapper;

        }

        public void Register(WebApplication application)
        {
            application.MapGet("/api/playlists", Get)
                .Produces<List<PlaylistDto>>(StatusCodes.Status200OK);

            application.MapGet("/api/playlists/{playlistId}", GetById)
                .Produces<Playlist>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            application.MapGet("/api/playlists/users/{userId}", GetByUserId)
                .Produces<List<PlaylistDto>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            application.MapPost("/api/playlists", Post)
                .Accepts<PlaylistDto>("application/json")
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status403Forbidden);

            application.MapPut("/api/playlists", Put)
                .Accepts<Playlist>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status403Forbidden);

            application.MapDelete("/api/playlists/{playlistId}", Delete)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status403Forbidden);
        }

        [AllowAnonymous]
        private async Task<IResult> Get(IPlaylistRepository repository)
        {
            var data = _mapper.Map<List<PlaylistDto>>(await _repository.GetPlaylistsAsync());
            data.ForEach(x => x.User = _mapper.Map<UserDto>(_userRepository.GetUserByIdAsync(x.ParentUserId).Result));
            return Results.Ok(data);
        }

        [AllowAnonymous]
        private async Task<IResult> GetById(Guid playlistId)
        {
            var playlist = _mapper.Map<PlaylistDto>(await _repository.GetPlaylistAsync(playlistId));
            if (playlist == null)
            {
                return Results.NotFound();
            }
            playlist.User = _mapper.Map<UserDto>(_userRepository.GetUserByIdAsync(playlist.ParentUserId).Result);
            return Results.Ok(playlist);
        }

        [Authorize]
        private async Task<IResult> GetByUserId(Guid userId)
        {
            var playlists = _mapper.Map<List<PlaylistDto>>(await _repository.GetUserPlaylists(userId));
            if (playlists == null)
            {
                return Results.NotFound("Not found");
            }
            return Results.Ok(playlists);
        }
            

        [Authorize]
        private async Task<IResult> Post([FromBody] PlaylistDto playlistDto)
        {
            var playlist = _mapper.Map<Playlist>(playlistDto);
            var validation = await _validator.ValidateAsync(playlist);
            if (!validation.IsValid)
            {
                return Results.ValidationProblem(validation.ToDictionary());
            }
            if (playlist.Image == string.Empty)
            {
                playlist.Image = "548864f8-319e-40ac-9f9b-a31f65ccb902.png";
            }
            playlist.Id = Guid.NewGuid();
            var user = await _userRepository.GetUserByIdAsync(playlist.ParentUserId);
            if (user == null)
            {
                return Results.BadRequest("User not found");
            }
            playlist.Users.Add(user);
            user.Playlists.Add(playlist);
            await _repository.InsertPlaylistAsync(playlist);
            await _repository.SaveAsync();
            return Results.Created($"$api/playlists/{playlist.Id}", playlist.Id);
        }

        [Authorize]
        private async Task<IResult> Put([FromBody] Playlist playlist, Guid userId)
        {
            var validation = await _validator.ValidateAsync(playlist);
            if (!validation.IsValid)
            {
                return Results.ValidationProblem(validation.ToDictionary());
            }
            if (playlist.ParentUserId != userId)
            {
                return Results.Forbid();
            }
            await _repository.UpdatePlaylistAsync(playlist);
            await _repository.SaveAsync();
            return Results.Ok();
        }

        [Authorize]
        private async Task<IResult> Delete(Guid playlistId, Guid userId)
        {
            var playlist = await _repository.GetPlaylistAsync(playlistId);
            if (playlist.ParentUserId != userId || playlist == null)
            {
                return Results.Forbid();
            }
            await _repository.DeletePlaylistAsync(playlistId);
            await _repository.SaveAsync();
            return Results.Ok();
        }
    }
}
