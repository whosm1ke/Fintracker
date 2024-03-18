using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Queries;

public class
    GetWalletsByOwnerIdSortedRequestHandler : IRequestHandler<GetWalletsByOwnerIdSortedRequest,
    IReadOnlyList<WalletBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<string> _allowedSortColumns;

    public GetWalletsByOwnerIdSortedRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _allowedSortColumns = new()
        {
            nameof(Domain.Entities.Wallet.Name).ToLowerInvariant(),
            nameof(Domain.Entities.Wallet.Balance).ToLowerInvariant()
        };
    }

    public async Task<IReadOnlyList<WalletBaseDTO>> Handle(GetWalletsByOwnerIdSortedRequest request,
        CancellationToken cancellationToken)
    {
        if (!_allowedSortColumns.Contains(request.Params.SortBy))
            throw new BadRequestException(new ExceptionDetails
            {
                PropertyName = nameof(request.Params.SortBy),
                ErrorMessage = $"Allowed values for sort by are {string.Join(", ", _allowedSortColumns)}"
            });

        var wallets =
            await _unitOfWork.WalletRepository.GetByOwnerIdSortedAsync(request.OwnerId, request.Params.SortBy,
                request.Params.IsDescending);


        return _mapper.Map<List<WalletBaseDTO>>(wallets);
    }
}