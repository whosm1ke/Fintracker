using Fintracker.Application.DTO.Common;
using Fintracker.Application.DTO.Currency;

namespace Fintracker.Application.DTO.Wallet;

public class WalletBaseDTO : IBaseDto
{
    public Guid Id { get; set; }

    public Guid OwnerId { get; set; }
    
    public string Name { get; set; }

    public decimal Balance { get; set; }
    
    public CurrencyDTO Currency { get; set; }
}