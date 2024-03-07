using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
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
            nameof(Domain.Entities.Wallet.Name),
            nameof(Domain.Entities.Wallet.Balance)
        };
    }

    public async Task<IReadOnlyList<WalletBaseDTO>> Handle(GetWalletsByOwnerIdSortedRequest request,
        CancellationToken cancellationToken)
    {
        if (!_allowedSortColumns.Contains(request.SortBy))
            throw new BadRequestException(
                $"Invalid sortBy parameter. Allowed values {string.Join(',', _allowedSortColumns)}");
        
        var wallets =
            await _unitOfWork.WalletRepository.GetByOwnerIdSortedAsync(request.OwnerId, request.SortBy,
                request.IsDescending);


        return _mapper.Map<List<WalletBaseDTO>>(wallets);
    }
}