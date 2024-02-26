using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Wallet;

public class UpdateWalletDTO : IBaseDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public decimal StartBalance { get; set; }

    public Guid CurrencyId { get; set; }
    
    public ICollection<Guid> UserIds { get; set; }
}