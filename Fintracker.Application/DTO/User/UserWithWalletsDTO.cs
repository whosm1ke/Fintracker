using Fintracker.Application.DTO.Wallet;

namespace Fintracker.Application.DTO.User;

public class UserWithWalletsDTO : UserBaseDTO
{
    public ICollection<WalletBaseDTO> OwnedWallets { get; set; }
    public ICollection<WalletBaseDTO> MemberWallets { get; set; }
}