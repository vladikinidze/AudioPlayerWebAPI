﻿using MediatR;

namespace AudioPlayerWebAPI.UseCase.Users.Commands.Register;

public class RegisterCommand : IRequest<Guid>
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}