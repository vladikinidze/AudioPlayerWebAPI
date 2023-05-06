namespace AudioPlayerWebAPI.Models.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly AudioPlayerDbContext _context;

        public UserRepository(AudioPlayerDbContext context)
        {
            _context = context;
        }


        public async Task<User> AutenticateUserAsync(UserDto userDto) => 
            (await _context.Users
                .FirstOrDefaultAsync(x => x.Email == userDto.Email && x.Password == Hash.GetSha1Hash(userDto.Password)))!;

        public async Task<User> GetUserByIdAsync(Guid userId) =>
            (await _context.Users.FindAsync(userId))!;

        public async Task<User> InsertUserAsync(UserDto userDto)
        {
            var userFromDb = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == userDto.Email);
            if (userFromDb != null) return null!;
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = userDto.Username,
                Email = userDto.Email,
                Password = Hash.GetSha1Hash(userDto.Password),
            };
            await _context.Users.AddAsync(user);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            var userFromDb = await _context.Users.FindAsync(user.Id);
            if (userFromDb == null) return;
            userFromDb.Username = user.Username;
            userFromDb.Image = user.Image;
            userFromDb.Email = user.Email;
            userFromDb.Password = user.Password;
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var userFromDb = await _context.Users.FindAsync(userId);
            if (userFromDb == null) return;
            _context.Users.Remove(userFromDb);
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
