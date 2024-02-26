﻿using Fintracker.Application.DTO.User;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Queries;

public class GetUsersAccessedToWalletRequest : IRequest<IReadOnlyList<UserBaseDTO>>
{
    public Guid WalletId { get; set; }
}