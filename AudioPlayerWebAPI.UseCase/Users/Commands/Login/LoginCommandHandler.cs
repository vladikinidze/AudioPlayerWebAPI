using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioPlayerWebAPI.UseCase.Interfaces;
using AudioPlayerWebAPI.UseCase.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerWebAPI.UseCase.User.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthViewModel>
    {
        private readonly IAudioPlayerDbContext _context;

        public LoginCommandHandler(IAudioPlayerDbContext context)
        {
            _context = context;
        }

        public async Task<AuthViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return new AuthViewModel();
        }
    }
}
