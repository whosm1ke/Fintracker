using Fintracker.Application.DTO.Transaction;

namespace Fintracker.Application.DTO.Wallet;

public class WalletWithTransactionsDTO : WalletPureDTO
{
    public ICollection<TransactionBaseDTO> Transactions { get; set; } = default!;
}