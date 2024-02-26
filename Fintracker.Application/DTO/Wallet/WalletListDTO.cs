using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Wallet;

public class WalletListDTO : IBaseDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public decimal Balance { get; set; }
}