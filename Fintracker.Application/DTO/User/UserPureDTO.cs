using Fintracker.Application.DTO.Common;
using Fintracker.Application.DTO.Currency;

namespace Fintracker.Application.DTO.User;

public class UserPureDTO : IBaseDto
{
    public Guid Id { get; set; }

    public string UserName { get; set; }
    
    public string Email { get; set; } = default!;

    public UserDetailsDTO? UserDetails { get; set; } = default!;
    
    public CurrencyDTO GlobalCurrency { get; set; }
}