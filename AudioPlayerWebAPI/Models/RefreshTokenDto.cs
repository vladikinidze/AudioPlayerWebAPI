using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Users.Commands.RefreshToken;
using AutoMapper;

namespace AudioPlayerWebAPI.Models
{
    public class RefreshTokenDto : IMap<RefreshTokenCommand>
    {
        public Guid? UserId { get; set; }  
        public string? AccessToken { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RefreshTokenDto, RefreshTokenCommand>()
                .ForMember(refreshTokenCommand => refreshTokenCommand.UserId,
                    opt => opt.MapFrom(refreshTokenDto => refreshTokenDto.UserId))
                .ForMember(refreshTokenCommand => refreshTokenCommand.AccessToken,
                    opt => opt.MapFrom(refreshTokenDto => refreshTokenDto.AccessToken))
                .ForMember(refreshTokenCommand => refreshTokenCommand.RefreshToken,
                    opt => opt.MapFrom(refreshTokenDto => refreshTokenDto.RefreshToken));
        }
    }
}
