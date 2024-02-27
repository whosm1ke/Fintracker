using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Budget.Handlers.Queries;

public class GetBudgetWithUserByIdRequestHandler : IRequestHandler<GetBudgetWithUserByIdRequest, BudgetWithUserDTO>
{    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBudgetWithUserByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<BudgetWithUserDTO> Handle(GetBudgetWithUserByIdRequest request, CancellationToken cancellationToken)
    {
        var budgets = await _unitOfWork.BudgetRepository.GetBudgetWithUserAsync(request.Id);

        //TODO: may be there should be some validation logic to ensure that list is not empty

        return _mapper.Map<BudgetWithUserDTO>(budgets);
    }
}