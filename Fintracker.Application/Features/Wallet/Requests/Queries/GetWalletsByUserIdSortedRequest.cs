using Fintracker.Application.DTO.Wallet;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Queries;

public class GetWalletsByUserIdSortedRequest : IRequest<IReadOnlyList<WalletBaseDTO>>
{
    public Guid UserId { get; set; }
    public string SortBy { get; set; }
}