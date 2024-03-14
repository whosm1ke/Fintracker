using Fintracker.Application.DTO.Wallet;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Queries;

public class GetWalletsByOwnerIdSortedRequest : IRequest<IReadOnlyList<WalletBaseDTO>>
{
    public Guid OwnerId { get; set; }
    public string SortBy { get; set; } = default!;
    
    public bool IsDescending { get; set; }
}