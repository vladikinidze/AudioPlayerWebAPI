using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Users.Queries.GetUserSettingsQuery;

public class GetUserSettingsQueryHandler : IRequestHandler<GetUserSettingsQuery, SettingsViewModel>
{
    private readonly IAudioPlayerDbContext _context;
    private readonly IMapper _mapper;

    public GetUserSettingsQueryHandler(IAudioPlayerDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<SettingsViewModel> Handle(GetUserSettingsQuery request, CancellationToken cancellationToken)
    {
        var settingsViewModel = await _context.Users
            .ProjectTo<SettingsViewModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        if (settingsViewModel == null)
        {
            throw new NotFoundException(nameof(User), request.UserId.ToString());
        }
        return settingsViewModel;
    }
}