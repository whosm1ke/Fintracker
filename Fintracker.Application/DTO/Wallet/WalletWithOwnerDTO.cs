using Fintracker.Application.DTO.User;

namespace Fintracker.Application.DTO.Wallet;

public class WalletWithOwnerDTO : WalletBaseDTO
{
    public UserBaseDTO Owner { get; set; }
}