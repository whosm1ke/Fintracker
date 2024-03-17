using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Wallet;

public class WalletPureDTO: IBaseDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    
    public string Name { get; set; } = default!;

    public decimal Balance { get; set; }
    
    public Guid CurrencyId { get; set; }
    
    public bool IsBanking { get; set; }

    public string BankAccountId { get; set; } = default!;
}