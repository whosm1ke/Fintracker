using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Budget.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Budget.Handlers.Queries;

public class GetBudgetWithWalletByIdRequestHandler : IRequestHandler<GetBudgetWithWalletByIdRequest, BudgetWithWalletDTO>
{    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBudgetWithWalletByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<BudgetWithWalletDTO> Handle(GetBudgetWithWalletByIdRequest request, CancellationToken cancellationToken)
    {
        var budget = await _unitOfWork.BudgetRepository.GetBudgetWithWalletAsync(request.Id);

        if (budget is null)
            throw new NotFoundException(nameof(Domain.Entities.Budget), request.Id);

        var b = _mapper.Map<BudgetWithWalletDTO>(budget);
        return b;
    }
}