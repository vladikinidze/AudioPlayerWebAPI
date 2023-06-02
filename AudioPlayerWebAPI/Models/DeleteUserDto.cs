using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Users.Commands.DeleteAccount;
using AutoMapper;

namespace AudioPlayerWebAPI.Models
{
    public class DeleteUserDto : IMap<DeleteAccountCommand>
    {
        public string Password { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteUserDto, DeleteAccountCommand>()
                .ForMember(deleteCommand => deleteCommand.Password,
                    opt => opt.MapFrom(deleteDto => deleteDto.Password));
        }
    }
}
