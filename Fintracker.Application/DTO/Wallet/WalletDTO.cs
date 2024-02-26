using Fintracker.Application.DTO.Common;
using Fintracker.Application.DTO.Transaction;

namespace Fintracker.Application.DTO.Wallet;

public class WalletDTO : IBaseDto
{
    public Guid Id { get; set; }
    
    public ICollection<Guid> UserIds { get; set; }

    public ICollection<TransactionDTO> Transactions { get; set; }

    public string Name { get; set; }

    public decimal StartBalance { get; set; }

    public Guid CurrencyId { get; set; }
}