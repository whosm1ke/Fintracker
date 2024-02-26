using Fintracker.Application.DTO.Wallet;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Queries;

public class GetWalletWithMembersByIdRequest : IRequest<WalletWithMembersDTO>
{
    public Guid Id { get; set; }
}