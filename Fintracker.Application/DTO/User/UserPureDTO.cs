using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.User;

public class UserPureDTO : IBaseDto
{
    public Guid Id { get; set; }

    public string UserName { get; set; }
    
    public string Email { get; set; } = default!;

    public UserDetailsDTO? UserDetails { get; set; } = default!;
}