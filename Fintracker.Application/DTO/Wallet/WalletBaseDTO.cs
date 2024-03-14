using Fintracker.Application.DTO.Currency;
using Fintracker.Application.DTO.User;

namespace Fintracker.Application.DTO.Wallet;

public class WalletBaseDTO : WalletPureDTO
{
    public UserBaseDTO Owner { get; set; } = default!;

    public CurrencyDTO Currency { get; set; } = default!;
}