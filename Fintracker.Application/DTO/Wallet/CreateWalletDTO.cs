namespace Fintracker.Application.DTO.Wallet;

public class CreateWalletDTO: IWalletDto
{
    public string Name { get; set; } = default!;

    public decimal Balance { get; set; }

    public Guid CurrencyId { get; set; }

    public Guid OwnerId { get; set; }
    
    public bool IsBanking { get; set; }
}