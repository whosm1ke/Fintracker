namespace Fintracker.Application.DTO.Wallet;

public interface IWalletDto
{
    public string Name { get; set; }

    public decimal Balance { get; set; }

    public Guid CurrencyId { get; set; }
}