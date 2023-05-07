namespace AudioPlayerWebAPI.Repositories
{
    public interface IUserRepository : IDisposable
    {
        Task<User> AutenticateUserAsync(UserDto userDto);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> InsertUserAsync(UserDto userDto);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid userId);
        Task SaveAsync();
    }
}
