using Fintracker.Application.DTO.User;
using Fintracker.Application.DTO.Wallet;

namespace Fintracker.Application.DTO.Transaction;

public class TransactionWithWalletAndUserDTO : TransactionBaseDTO
{
    public WalletBaseDTO Wallet { get; set; }

    public UserBaseDTO User { get; set; }
}