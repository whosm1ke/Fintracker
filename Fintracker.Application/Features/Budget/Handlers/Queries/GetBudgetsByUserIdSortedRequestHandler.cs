using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Budget.Handlers.Queries;

public class
    GetBudgetsByUserIdSortedRequestHandler : IRequestHandler<GetBudgetsByUserIdSortedRequest,
    IReadOnlyList<BudgetBaseDTO>>
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBudgetsByUserIdSortedRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IReadOnlyList<BudgetBaseDTO>> Handle(GetBudgetsByUserIdSortedRequest request,
        CancellationToken cancellationToken)
    {
        var budgets = await _unitOfWork.BudgetRepository.GetByUserIdSortedAsync(request.UserId, request.SortBy);

        //TODO: may be there should be some validation logic to ensure that list is not empty

        return _mapper.Map<List<BudgetBaseDTO>>(budgets);
    }
}