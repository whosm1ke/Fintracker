using Fintracker.Application.DTO.User;

namespace Fintracker.Application.DTO.Wallet;

public class WalletWithMembersDTO : WalletPureDTO
{
    public ICollection<UserBaseDTO> Users { get; set; } = default!;
}