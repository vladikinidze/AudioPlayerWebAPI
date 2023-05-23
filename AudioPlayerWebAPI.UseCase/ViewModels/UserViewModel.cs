using AudioPlayerWebAPI.Entities;
using AutoMapper;

namespace AudioPlayerWebAPI.UseCase.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; }
        public string? Image { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserViewModel>()
                .ForMember(userViewModel => userViewModel.Id,
                    opt => opt.MapFrom(user => user.Id))
                .ForMember(userViewModel => userViewModel.Username,
                    opt => opt.MapFrom(user => user.Username))
                .ForMember(userViewModel => userViewModel.Email,
                    opt => opt.MapFrom(user => user.Email))
                .ForMember(userViewModel => userViewModel.Image,
                    opt => opt.MapFrom(user => user.Image));
        }
    }
}
