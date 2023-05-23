using AudioPlayerWebAPI.Services;
using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Users.Commands.DeleteAccount;
using AutoMapper;

namespace AudioPlayerWebAPI.Models
{
    public class DeleteUserDto : IMap<DeleteAccountCommand>
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteUserDto, DeleteAccountCommand>()
                .ForMember(deleteCommand => deleteCommand.Password,
                    opt => opt.MapFrom(deleteDto => Hash.GetSha1Hash(deleteDto.Password)));
        }
    }
}
