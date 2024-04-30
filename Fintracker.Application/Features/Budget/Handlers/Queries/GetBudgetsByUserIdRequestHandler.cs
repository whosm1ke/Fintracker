using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Budget.Handlers.Queries;

public class GetBudgetsByUserIdRequestHandler : IRequestHandler<GetBudgetsByUserIdRequest, IReadOnlyList<BudgetBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBudgetsByUserIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<BudgetBaseDTO>> Handle(GetBudgetsByUserIdRequest request,
        CancellationToken cancellationToken)
    {
        var budgetsByUserId =
            await _unitOfWork.BudgetRepository.GetByBudgetUserIdAsync(request.UserId, request.IsPublic);


        return _mapper.Map<List<BudgetBaseDTO>>(budgetsByUserId);

    }
}