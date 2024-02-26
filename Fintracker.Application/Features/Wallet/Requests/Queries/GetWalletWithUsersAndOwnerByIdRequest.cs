using Fintracker.Application.DTO.Wallet;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Queries;

public class GetWalletWithUsersAndOwnerByIdRequest : IRequest<WalletDTO>
{
    public Guid Id { get; set; }
}