namespace Fintracker.Application.DTO.Invite;

public class AcceptInviteDto
{
    public Guid WalletId { get; set; }
    
    public Guid UserId { get; set; }
}