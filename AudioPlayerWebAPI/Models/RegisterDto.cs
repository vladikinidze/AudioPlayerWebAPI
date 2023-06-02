using AutoMapper;
using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Users.Commands.Register;
using AudioPlayerWebAPI.Services;

namespace AudioPlayerWebAPI.Models
{
    public class RegisterDto : IMap<RegisterCommand>
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterDto, RegisterCommand>()
                .ForMember(registerCommand => registerCommand.UserName,
                    opt => opt.MapFrom(registerDto => registerDto.Username))
                .ForMember(registerCommand => registerCommand.Email,
                    opt => opt.MapFrom(registerDto => registerDto.Email))
                .ForMember(registerCommand => registerCommand.Password,
                    opt => opt.MapFrom(registerDto => registerDto.Password))
                .ForMember(registerCommand => registerCommand.ConfirmPassword,
                    opt => opt.MapFrom(registerDto => registerDto.ConfirmPassword));

        }
    }
}
