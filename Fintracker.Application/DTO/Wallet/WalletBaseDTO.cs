using Fintracker.Application.DTO.Common;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.DTO.User;

namespace Fintracker.Application.DTO.Wallet;

public class WalletBaseDTO : IBaseDto
{
    public Guid Id { get; set; }

    public UserBaseDTO Owner { get; set; }
    
    public string Name { get; set; }

    public decimal Balance { get; set; }
    
    public CurrencyDTO Currency { get; set; }
}