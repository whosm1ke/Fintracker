﻿using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Commands;

public class CreateBudgetCommand : IRequest<CreateCommandResponse<CreateBudgetDTO>>
{
    public CreateBudgetDTO Budget { get; set; }
}