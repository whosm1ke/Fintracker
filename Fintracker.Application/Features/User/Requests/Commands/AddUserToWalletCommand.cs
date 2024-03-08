﻿using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class AddUserToWalletCommand : IRequest<BaseCommandResponse>
{
    public Guid WalletId { get; set; }

    public string Token { get; set; }
}