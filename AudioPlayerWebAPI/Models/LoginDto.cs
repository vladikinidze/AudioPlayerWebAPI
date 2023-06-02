using AudioPlayerWebAPI.Services;
using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Users.Commands.Login;
using AutoMapper;

namespace AudioPlayerWebAPI.Models
{
    public class LoginDto : IMap<LoginCommand>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LoginDto, LoginCommand>()
                .ForMember(loginCommand => loginCommand.Email,
                    opt => opt.MapFrom(loginDto => loginDto.Email))
                .ForMember(loginCommand => loginCommand.Password,
                    opt => opt.MapFrom(loginDto => loginDto.Password));
        }
    }
}
