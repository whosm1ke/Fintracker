using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Queries;

public class
    GetWalletWithBudgetsByIdRequestHandler : IRequestHandler<GetWalletWithBudgetsByIdRequest, WalletWithBudgetsDTO>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetWalletWithBudgetsByIdRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<WalletWithBudgetsDTO> Handle(GetWalletWithBudgetsByIdRequest request,
        CancellationToken cancellationToken)
    {
        var wallet = await _unitOfWork.WalletRepository.GetWalletWithBudgetsAsync(request.Id);

        if (wallet is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by id [{request.Id}]",
                PropertyName = nameof(request.Id)
            },nameof(Domain.Entities.Wallet));

        return _mapper.Map<WalletWithBudgetsDTO>(wallet);
    }
}