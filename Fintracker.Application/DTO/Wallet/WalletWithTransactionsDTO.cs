using Fintracker.Application.DTO.Transaction;

namespace Fintracker.Application.DTO.Wallet;

public class WalletWithTransactionsDTO : WalletBaseDTO
{
    public ICollection<TransactionBaseDTO> Transactions { get; set; } = default!;
}