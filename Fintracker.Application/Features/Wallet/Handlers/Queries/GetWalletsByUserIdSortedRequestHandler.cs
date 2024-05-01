using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Queries;

public class
    GetWalletsByUserIdSortedRequestHandler : IRequestHandler<GetWalletsByUserIdSortedRequest,
    IReadOnlyList<WalletBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<string> _allowedSortColumns;

    public GetWalletsByUserIdSortedRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _allowedSortColumns = new()
        {
            nameof(Domain.Entities.Wallet.Name).ToLowerInvariant(),
            nameof(Domain.Entities.Wallet.Id).ToLowerInvariant(),
            nameof(Domain.Entities.Wallet.Balance).ToLowerInvariant()
        };
    }

    public async Task<IReadOnlyList<WalletBaseDTO>> Handle(GetWalletsByUserIdSortedRequest request,
        CancellationToken cancellationToken)
    {
        if (!_allowedSortColumns.Contains(request.Params.SortBy))
            throw new BadRequestException(new ExceptionDetails
            {
                PropertyName = nameof(request.Params.SortBy),
                ErrorMessage = $"Allowed values for sort by are {string.Join(", ", _allowedSortColumns)}"
            });


        var wallets =
            await _unitOfWork.WalletRepository.GetByUserIdSortedAsync(request.UserId, request.Params);


        return _mapper.Map<List<WalletBaseDTO>>(wallets);
    }
}