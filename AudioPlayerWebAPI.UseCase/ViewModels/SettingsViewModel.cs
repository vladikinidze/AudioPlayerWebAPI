using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Mapping;
using AutoMapper;

namespace AudioPlayerWebAPI.UseCase.ViewModels;

public class SettingsViewModel : IMap<Settings>
{
    public bool Explicit { get; set; }
    public Guid UserId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Settings, SettingsViewModel>()
            .ForMember(viewModel => viewModel.Explicit,
                opt => opt.MapFrom(settings => settings.Explicit))
            .ForMember(viewModel => viewModel.UserId,
                opt => opt.MapFrom(settings => settings.User.Id));
    }
}