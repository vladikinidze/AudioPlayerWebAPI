using AudioPlayerWebAPI.UseCase.Errors;
using AudioPlayerWebAPI.UseCase.Mapping;
using AutoMapper;

namespace AudioPlayerWebAPI.Models;

public class ErrorDto : IMap<ErrorCommand>
{
    public string? Text { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ErrorDto, ErrorCommand>()
            .ForMember(x => x.Text,
                opt => opt.MapFrom(x => x.Text));
    }
}