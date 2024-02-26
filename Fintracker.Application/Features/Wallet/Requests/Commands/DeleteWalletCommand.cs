﻿using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Commands;

public class DeleteWalletCommand : IRequest<DeleteCommandResponse<WalletBaseDTO>>
{
    public Guid Id { get; set; }
}