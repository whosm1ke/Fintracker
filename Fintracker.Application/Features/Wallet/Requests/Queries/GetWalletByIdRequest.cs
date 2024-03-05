using Fintracker.Application.DTO.Wallet;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Requests.Queries;

public class GetWalletByIdRequest : IRequest<WalletBaseDTO>
{
    public Guid Id { get; set; }
}