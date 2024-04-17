using Fintracker.Application.DTO.Budget;
using MediatR;

namespace Fintracker.Application.Features.Budget.Requests.Queries;

public class GetBudgetsWithWalletsRequest : IRequest<List<BudgetWithWalletDTO>>
{

}