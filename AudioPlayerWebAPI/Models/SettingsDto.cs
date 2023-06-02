using AudioPlayerWebAPI.UseCase.Mapping;
using AudioPlayerWebAPI.UseCase.Users.Commands.SetSettings;
using AutoMapper;

namespace AudioPlayerWebAPI.Models;

public class SettingsDto : IMap<SetSettingsCommand>
{
    public bool Explicit { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SettingsDto, SetSettingsCommand>()
            .ForMember(viewModel => viewModel.Explicit,
                opt => opt.MapFrom(settings => settings.Explicit));
    }
}