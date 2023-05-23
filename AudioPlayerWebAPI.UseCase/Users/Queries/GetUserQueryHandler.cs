using AudioPlayerWebAPI.Entities;
using AudioPlayerWebAPI.UseCase.Exceptions;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.Users.Queries
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserViewModel>
    {
        private readonly IAudioPlayerDbContext _context;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IAudioPlayerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserViewModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken: cancellationToken);

            return user ?? throw new NotFoundException(nameof(User), request.Id.ToString());
        }
    }
}
