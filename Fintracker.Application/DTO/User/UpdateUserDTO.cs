using Fintracker.Application.DTO.Common;
using Microsoft.AspNetCore.Http;

namespace Fintracker.Application.DTO.User;

public class UpdateUserDTO : IBaseDto
{
    public Guid Id { get; set; }
    
    public string Email { get; set; } = default!;
    
    public UserDetailsDTO UserDetails { get; set; } = default!;

    public IFormFile? Avatar { get; set; }
    
    public string GlobalCurrency { get; set; }

}