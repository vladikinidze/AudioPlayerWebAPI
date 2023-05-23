using AudioPlayerWebAPI.Services;
using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Users.Commands.UpdateAccount;
using AutoMapper;

namespace AudioPlayerWebAPI.Models
{
    public class UpdateUserDto : IMap<UpdateAccountCommand>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public IFormFile? Image { get; set; }
        public string Password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserDto, UpdateAccountCommand>()
                .ForMember(loginCommand => loginCommand.Email,
                    opt => opt.MapFrom(loginDto => loginDto.Email))
                .ForMember(loginCommand => loginCommand.Username,
                    opt => opt.MapFrom(loginDto => loginDto.Username))
                .ForMember(loginCommand => loginCommand.Password,
                    opt => opt.MapFrom(loginDto => Hash.GetSha1Hash(loginDto.Password)));
        }
    }
}
