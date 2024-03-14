namespace Fintracker.Application.DTO.Invite;

public class InviteDTO
{
    public string Email { get; set; } = default!;
    public Guid WalletId { get; set; }

    public string UrlCallback { get; set; } = default!;
}