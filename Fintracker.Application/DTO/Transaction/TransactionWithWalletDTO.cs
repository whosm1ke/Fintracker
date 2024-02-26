using Fintracker.Application.DTO.Wallet;

namespace Fintracker.Application.DTO.Transaction;

public class TransactionWithWalletDTO : TransactionBaseDTO
{
    public WalletBaseDTO Wallet { get; set; }
}