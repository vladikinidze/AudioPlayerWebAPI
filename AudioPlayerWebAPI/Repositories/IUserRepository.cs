﻿namespace AudioPlayerWebAPI.Repositories
{
    public interface IUserRepository : IDisposable
    {
        Task<User> AutenticateUserAsync(LoginDto userDto);
        Task<User> RegisterateUserAsync(RegisterDto userDto);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid userId);
        Task SaveAsync();
    }
}
