﻿using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Commands;

public class UpdateWalletCommand : IRequest<UpdateCommandResponse<WalletBaseDTO>>
{
    public UpdateWalletDTO Wallet { get; set; }
}