namespace Fintracker.Application.DTO.Wallet;

public class CreateWalletDTO: IWalletDto
{
    public string Name { get; set; } = default!;

    public decimal StartBalance { get; set; }

    public Guid CurrencyId { get; set; }

    public Guid OwnerId { get; set; }
}