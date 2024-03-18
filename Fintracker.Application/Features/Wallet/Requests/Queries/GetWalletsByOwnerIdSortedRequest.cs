using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Models;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Queries;

public class GetWalletsByOwnerIdSortedRequest : IRequest<IReadOnlyList<WalletBaseDTO>>
{
    public Guid OwnerId { get; set; }
    public QueryParams Params { get; set; } = default!;
}