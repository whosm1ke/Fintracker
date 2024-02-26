using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.Common;
using Fintracker.Application.DTO.Wallet;

namespace Fintracker.Application.DTO.User;

public class UserDTO : IBaseDto
{
    public Guid Id { get; set; }
    
    public ICollection<WalletDTO> Wallets { get; set; }
    
    public ICollection<BudgetDTO> Budgets { get; set; }

    public string Email { get; set; }

    public UserDetailsDTO UserDetails { get; set; }
}