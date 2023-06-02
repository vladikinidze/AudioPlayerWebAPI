using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Users.Queries.GetUserSettingsQuery;

public class GetUserSettingsQuery : IRequest<SettingsViewModel>
{
    public Guid UserId { get; set; }
}