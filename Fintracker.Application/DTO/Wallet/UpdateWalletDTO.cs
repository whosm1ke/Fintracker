using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Wallet;

public class UpdateWalletDTO : IBaseDto, IWalletDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;

    public decimal StartBalance { get; set; }

    public Guid CurrencyId { get; set; }

    public IEnumerable<Guid> UserIds { get; set; }

    
}