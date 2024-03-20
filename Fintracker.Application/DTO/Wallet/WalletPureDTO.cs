using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Wallet;

public class WalletPureDTO: FinancialEntityDTO
{
    public Guid OwnerId { get; set; }
    
    public bool IsBanking { get; set; }

    public string BankAccountId { get; set; } = default!;
}