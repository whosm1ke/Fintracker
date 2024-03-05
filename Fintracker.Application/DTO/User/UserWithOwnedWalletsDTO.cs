using Fintracker.Application.DTO.Wallet;

namespace Fintracker.Application.DTO.User;

public class UserWithOwnedWalletsDTO : UserBaseDTO
{
    public ICollection<WalletBaseDTO> OwnedWallets { get; set; }
}