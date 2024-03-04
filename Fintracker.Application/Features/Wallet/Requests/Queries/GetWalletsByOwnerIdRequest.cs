using Fintracker.Application.DTO.Wallet;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Queries;

public class GetWalletsByOwnerIdRequest : IRequest<IReadOnlyList<WalletBaseDTO>>
{
    public Guid OwnerId { get; set; }
}