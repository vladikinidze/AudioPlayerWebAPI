using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.DeleteAccount
{
    public class DeleteAccountCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}
