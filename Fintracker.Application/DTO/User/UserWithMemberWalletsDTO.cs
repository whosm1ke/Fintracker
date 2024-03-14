using Fintracker.Application.DTO.Wallet;

namespace Fintracker.Application.DTO.User;

public class UserWithMemberWalletsDTO : UserBaseDTO
{
    public ICollection<WalletBaseDTO> MemberWallets { get; set; } = default!;
}