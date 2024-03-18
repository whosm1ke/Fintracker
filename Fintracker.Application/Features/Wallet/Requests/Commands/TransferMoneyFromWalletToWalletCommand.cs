﻿using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Commands;

public class TransferMoneyFromWalletToWalletCommand : IRequest<BaseCommandResponse>
{
    public Guid FromWalletId { get; set; }
    public Guid ToWalletId { get; set; }
    public decimal Amount { get; set; }
}