using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Budget.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Budget.Handlers.Queries;

public class
    GetBudgetsByWalletIdSortedRequestHandler : IRequestHandler<GetBudgetsByWalletIdSortedRequest,
    IReadOnlyList<BudgetBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private List<string> _allowedSortColumns;

    public GetBudgetsByWalletIdSortedRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _allowedSortColumns = new List<string>
        {
            nameof(Domain.Entities.Budget.Name),
            nameof(Domain.Entities.Budget.StartDate),
            nameof(Domain.Entities.Budget.EndDate),
            nameof(Domain.Entities.Budget.Balance)
        };
    }

    public async Task<IReadOnlyList<BudgetBaseDTO>> Handle(GetBudgetsByWalletIdSortedRequest request,
        CancellationToken cancellationToken)
    {
        if (!_allowedSortColumns.Contains(request.SortBy))
            throw new BadRequestException(
                $"Invalid sortBy parameter. Allowed values {string.Join(',', _allowedSortColumns)}");

        var budgets =
            await _unitOfWork.BudgetRepository.GetByWalletIdSortedAsync(request.WalletId, request.SortBy,
                request.IsDescending);


        return _mapper.Map<List<BudgetBaseDTO>>(budgets);
    }
}