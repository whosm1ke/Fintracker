using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Budget.Handlers.Queries;

public class
    GetBudgetsByWalletIdRequestHandler : IRequestHandler<GetBudgetsByWalletIdRequest, IReadOnlyList<BudgetBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBudgetsByWalletIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<BudgetBaseDTO>> Handle(GetBudgetsByWalletIdRequest request,
        CancellationToken cancellationToken)
    {
        var budgets = await _unitOfWork.BudgetRepository.GetByWalletIdAsync(request.WalletId,
            request.IsPublic);


        return _mapper.Map<List<BudgetBaseDTO>>(budgets);
    }
}