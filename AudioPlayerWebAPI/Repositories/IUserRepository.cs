namespace AudioPlayerWebAPI.Repositories
{
    public interface IUserRepository : IDisposable
    {
        Task<User> AutenticateUserAsync(UserDto userDto);
        Task<User> RegisterateUserAsync(UserDto userDto);
        Task<User> GetUserByIdAsync(Guid userId);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid userId);
        Task SaveAsync();
    }
}
