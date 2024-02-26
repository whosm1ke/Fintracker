using Fintracker.Application.DTO.Wallet;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Queries;

public class GetWalletWithOwnerByIdRequest : IRequest<WalletWithOwnerDTO>
{
    public Guid Id { get; set; }
}