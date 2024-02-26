using Fintracker.Application.DTO.User;

namespace Fintracker.Application.DTO.Wallet;

public class WalletWithMembersDTO : WalletBaseDTO
{
    public ICollection<UserBaseDTO> Users { get; set; }
}