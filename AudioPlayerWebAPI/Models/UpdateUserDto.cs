using AudioPlayerWebAPI.Services;
using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Users.Commands.UpdateAccount;
using AutoMapper;

namespace AudioPlayerWebAPI.Models
{
    public class UpdateUserDto : IMap<UpdateAccountCommand>
    {
        public string? Username { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public IFormFile? Image { get; set; }
        public bool EmptyImage { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserDto, UpdateAccountCommand>()
                .ForMember(loginCommand => loginCommand.Email,
                    opt => opt.MapFrom(loginDto => loginDto.Email))
                .ForMember(loginCommand => loginCommand.Username,
                    opt => opt.MapFrom(loginDto => loginDto.Username));
        }
    }
}
