using Fintracker.Application.DTO.Wallet;

namespace Fintracker.Application.DTO.User;

public class UserWithWalletsDTO : UserBaseDTO
{
    public ICollection<WalletBaseDTO> Wallets { get; set; }
}