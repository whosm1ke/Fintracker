using Fintracker.Application.DTO.Wallet;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Queries;

public class GetWalletsByUserIdRequest : IRequest<IReadOnlyList<WalletBaseDTO>>
{
    public Guid UserId { get; set; }
}